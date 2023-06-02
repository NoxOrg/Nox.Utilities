using System.Text.RegularExpressions;
using Nox.Utilities.Exceptions;

namespace Nox.Utilities.Yaml;

public class YamlResolver
{
    private readonly Regex _referenceRegex = new(@"\$ref\S*:\s*(?<variable>[\w:\.\/\\]+\b[\w\-\.\/]+)\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));
    private string? _sourceFile;
    private string? _sourceDirectory;

    public YamlResolver(string sourceFile)
    {
        if (string.IsNullOrEmpty(sourceFile)) throw new NoxYamlException("sourcePath cannot be an empty string!");
        var sourceFullPath = Path.GetFullPath(sourceFile);
        if (!File.Exists(sourceFullPath)) throw new NoxYamlException($"Yaml file {sourceFullPath} does not exist!");
        _sourceFile = sourceFullPath;
        _sourceDirectory = Path.GetDirectoryName(_sourceFile);
    }

    public string ResolveReferences()
    {
        var sourceLines = File.ReadAllLines(_sourceFile!);
        var outputLines = ResolveReferencesInternal(sourceLines.ToList());
        return string.Join("\n", outputLines.ToArray());
    }

    private List<string> ResolveReferencesInternal(List<string> sourceLines)
    {
        var outputLines = new List<string>();
        foreach (var sourceLine in sourceLines)
        {
            if (!sourceLine.TrimStart().StartsWith("#"))
            {
                var match = _referenceRegex.Match(sourceLine);
                if (match.Success)
                {
                    var padding = new string(' ', match.Index);
                    var prefixPadding = "";
                    var prefix = sourceLine.Substring(0, match.Index);
                    if (prefix.Contains('-')) prefixPadding = new string(' ', prefix.IndexOf('-'));
                    var childPath = match.Groups[1].Value;
                    if (!Path.IsPathRooted(childPath)) childPath = Path.Combine(_sourceDirectory!, childPath);
                    if (!File.Exists(childPath)) throw new NoxYamlException($"Referenced yaml file does not exist for reference: {match.Groups[1].Value}");
                    var childLines = File.ReadAllLines(childPath);
                    foreach ((var childLine, bool isFirst) in childLines.Select((childLine, index) => (childLine, index == 0)))
                    {
                        if (isFirst)
                        {
                            outputLines.Add((prefix + childLine).Replace("- -", "-"));
                        }
                        else
                        {
                            if (childLine.StartsWith("-"))
                            {
                                outputLines.Add(prefixPadding + childLine);
                            }
                            else
                            {
                                outputLines.Add(padding + childLine);
                            }
                            
                        }
                    }
                }
                else
                {
                    outputLines.Add(sourceLine);
                }
            }
            else
            {
                outputLines.Add(sourceLine);
            }
        }

        if (outputLines.Any(ol => ol.Contains("$ref:") && !ol.TrimStart().StartsWith("#")))
        {
            outputLines = ResolveReferencesInternal(outputLines);    
        }
        
        return outputLines;
    }
}
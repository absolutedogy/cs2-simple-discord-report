namespace ReportDcPlugin;

public class MessageInterpolation
{

    public static string InterpolateString(string template, Dictionary<string, string> context)
    {
        var inToken = false;
        var result = "";
        var currToken = "";

        for (int i = 0; i < template.Length; i++)
        {
            if (template[i] == '{')
            {
                // Handle escape sequence in case the template contains a { in the final message
                if (template[i + 1] == '{')
                {
                    i++;
                    result += "{";
                }
                else
                {
                    inToken = true;
                }
                continue;
            }

            if (template[i] == '}' && inToken)
            {
                inToken = false;

                // If we find the key in our context, ignoring case, append it to the result
                if (context.Keys.Count(k => string.Compare(k.Trim(), currToken.Trim(), StringComparison.OrdinalIgnoreCase) == 0) > 0)
                {
                    result += context[currToken];
                }
                else
                {
                    // Append a ? to indicate that something should have been placed here but for some reason wasn't
                    result += "?";
                }
                
                currToken = "";
                continue;
            }
            
            if (inToken)
            {
                currToken += template[i];
            }
            else
            {
                result += template[i];
            }

        }

        return result;
    }
}


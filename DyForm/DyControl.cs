using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyForm
{
    class DyControl
    {
        public string Type { get; set; }
        public string ID { get; set; }
        public string Label { get; set; }
        public object Value { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
        public string Matches { get; set; }
        public int TabIndex { get; set; }
        public int Max { get; set; }
        public int MaxLength { get; set; }
        public int Min { get; set; }
        public int Size { get; set; }
        public string Model { get; set; }

        public List<string> Validations = new List<string>();


        private const string regexAndMaxLengthMatchTemplate = @"<script>function checkRegexAndMaxFor{TextID}()
	{
		var x, text;

		x = document.getElementById(""{TextID}"").value;
        var patt = new RegExp(""{Matches}"");
		// If x is Not a Number or less than one or greater than 10
		var cond = patt.test(x);
		if (!cond)
		{
			text = ""Uhh! that's not a good value"";
			document.getElementById(""submitButton"").disabled = true;
		}
	
		else 
		{
			text = """";
			document.getElementById(""submitButton"").disabled = false;
		}
		document.getElementById(""{ErrorLabelID}"").innerHTML = text;
	}</script>";

        private const string checkFunctionTemplate = @"<script>function checkRangeFor{TextID}()
	{
		var x, text;

		x = document.getElementById(""{TextID}"").value;

		// If x is Not a Number or less than one or greater than 10
		if (isNaN(x) || x < {MinValue} || x > {MaxValue})
		{
			text = ""Value should be between {MinValue} and {MaxValue}"";
			document.getElementById(""submitButton"").disabled = true;
		}
		else {text = """";document.getElementById(""submitButton"").disabled = false;}
		document.getElementById(""{ErrorLabelID}"").innerHTML = text;
	}</script>";

        private void UpdateValidationScripts()
        {
            if (Type == "text")
            {
                //Max Length 
                //Pattern
                Validations.Add(regexAndMaxLengthMatchTemplate.Replace("{TextID}", ID)
                                       .Replace("{Matches}", Matches)
                                       .Replace("{MaxLength}", MaxLength.ToString())
                                       .Replace("{ErrorLabelID}", ID + "ErrorLabel"));

            }

            if (Type == "number")
            {
                Validations.Add(checkFunctionTemplate.Replace("{TextID}", ID)
                                       .Replace("{MinValue}", Min.ToString())
                                       .Replace("{MaxValue}", Max.ToString())
                                       .Replace("{ErrorLabelID}", ID + "ErrorLabel"));

            }
        }
        private string RenderText()
        {

            if (Required)
                return $"<tr><td>{Label}</td><td><input  type = \"text\" id = \"{ID}\"" +
                         $" ng-model=\"{ID}\" maxlength={MaxLength} pattern=\"{Matches}\" size={Size}" +
                         $" onchange =\"checkRegexAndMaxFor{ID}()\" required></td><td><label id=\"{ID + "ErrorLabel\""} style=\"color:red;\"/></td></tr>";
            else
                return $"<tr><td>{Label}</td><td><input  type = \"text\" id = \"{ID}\"" +
                         $" ng-model=\"{ID}\" maxlength={MaxLength} pattern=\"{Matches}\" size={Size}" +
                         $" onchange =\"checkRegexAndMaxFor{ID}()\"></td><td><label id =\"{ID + "ErrorLabel\""} style=\"color:red;\"/></td></tr>";

        }

        private string RenderNumericControl()
        {
            if (Required)
                return $"<tr><td>{Label}</td><td><input  type = \"number\" id = \"{ID}\"" +
                         $" maxlength={MaxLength} pattern=\"{Matches}\" size={Size}" +
                         $" onchange =\"checkRangeFor{ID}()\" required></td><td><label id =\"{ID + "ErrorLabel\""} style=\"color:red;\"/></td></tr>";
            else
                return $"<tr><td>{Label}</td><td><input  type = \"number\" id = \"{ID}\"" +
                         $" maxlength={MaxLength} pattern=\"{Matches}\" size={Size}" +
                         $" onchange =\"checkRangeFor{ID}()\"></td><td><label id =\"{ID + "ErrorLabel\""} style=\"color:red;\"/></td></tr>";
        }
        private string RenderEnumeration()
        {
            List<string> values = new List<string>();
            Newtonsoft.Json.Linq.JArray v = (Newtonsoft.Json.Linq.JArray)Value;
            for (int i = 0; i < v.Count; i++)
                values.Add(v[i].ToString());
            return $"<tr><td>{Label}</td><td><select name=\"{Label}\">" +
                     values.Select(va => $"<option value =\"{va}\">{va}</option>")
                     .Aggregate((a, b) => a + Environment.NewLine + b)
                   + "</select></td><td></td></tr>";
        }
        public string Render()
        {

            if (Type == "text")
            {
                UpdateValidationScripts();
                return RenderText();
            }
            if (Type == "number")
            {
                UpdateValidationScripts();
                return RenderNumericControl();
            }
            if (Type == "enumerated")
                return RenderEnumeration();

            if (Type == "conjunctive")
            {
                return $"<tr><td>{Label}</td><td><label>{Model.Replace("*", "{{").Replace("_", "}}")}</label></td><td></td></tr>";
            }
            return string.Empty;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyForm
{
    class DyForm
    {
        const string template = @"<!DOCTYPE html>
	<html ng-app=""myApp"" ng-controller=""myCtrl"">
	<head>
	<script src=""http://ajax.googleapis.com/ajax/libs/angularjs/1.4.8/angular.min.js""></script>	
	<script>
    var app = angular.module('myApp', []);
    app.controller('myCtrl', function($scope)
    {
    });
    </script>
    
	{{ValidationScripts}}	
	</head>
    <body>
    <h2>{{Name}}</h2>
	<form name=""dyForm"">
	<table>
		{{Controls}}
	</table>
		<input type=""Submit"" id=""submitButton""/>
	</form>
	
	<p id=""results""></p>
	</body>
	</html>
	";
        public List<DyControl> Controls =
                       new List<DyControl>();

        public string Name { get; set; }
        public string ID { get; set; }

        public List<KeyValuePair<string, string>> Post()
        {
            return new List<KeyValuePair<string, string>>();
        }

        public static DyForm CreateFromJson(string jsonRepresentation)
        {
            return JsonConvert.DeserializeObject<DyForm>(jsonRepresentation);
        }

        public string Render()
        {


            string controls = template.Replace("{{Name}}", Name)
                           .Replace("{{Controls}}", Controls.Select(c => c.Render())
                           .Aggregate((f, s) => f + Environment.NewLine + s));
            string form = controls.Replace("{{ValidationScripts}}",
                                    Controls.SelectMany(c => c.Validations)
                                            .Aggregate((a, b) => a + Environment.NewLine + b));

            return form;
        }
    }
}

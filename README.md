# BabyDyno
Dynamic Form Generator

### Representing a form using JSON

BabyDyno is a C# console app that takes a JSON and generates a HTML form. The controls can be either of these three types 

`text`
`number` 
`enumerated`. 

The program can add validations of range for number fields and for text fields the validations are of regular expression check. 

### Fields of each field 

These can be attributes of each field of control. 

* ID  _ ID of the field 
* Label _ Label of the field 
* Matches _ A regular expression that has to be matched for the `text` field 
* Required _ A boolean field that indicates whether the field is required or not 
* Enabled _ Whether the field is enabled or not
* Model _ This has to only specified for fields that rely on other fields 

### Model representation 

A model has to be marked with a * at the front and an underscore at the back as shown below 

``` "Model" : "*Description_ *CreatedBy_ " ```

At runtime this will be represented by binding using Angular JS 

### A sample case (Bug Report) 

Field Definitions
CreatedBy(text)
Description(text)
Severity(number)
Status(enumerated) - [CANCELLED, COMPLETED]
CancelledReason(enumerated) - [ENDUSER, OTHERS]
CancelledOtherDescription(text)
Comments(text)

Use case:  User completing a ticket. Convention: FIELD[VALUE(if the field has a value the user has selected)]
1. OnLoad 
Active Fields - [CreatedBy, Description, Severity, Status]
 User Enters a description - 'Internet is not working'
Active Fields = [CreatedBy, Description['Internet is not working'], Severity, Status]
User selects status as 'COMPLETED'
Active Fields = [CreatedBy, Description['Internet is not working'], Severity, Status['COMPLETED'], Comments]

Use case: User cancelling a ticket
OnLoad 
Active Fields - [CreatedBy, Description, Severity, Status]
User Enters a description - 'Internet is not working'
Active Fields = [CreatedBy, Description['Internet is not working'], Severity, Status]
User selects status as 'CANCELLED'
Active Fields = [CreatedBy, Description['Internet is not working'], Severity, Status['CANCELLED'], CancelledReason], 
User selects CancelledReason as OTHERS.
[CreatedBy, Description['Internet is not working'], Severity, Status['CANCELLED'], CancelledReason[OTHERS], CancelledOtherDescription]

This can be represented by the following JSON 

```json


{
			  "Controls": [
			    {
					"Type": "text",
			      "ID": "CreatedBy",				  
			      "Label": "Created by",
			      "Value": "",
			      "Required": true,
			      "Enabled": true,
			      "Matches": "[a-z ]+",
				  "MaxLength":20,
			      "TabIndex": 1
			    },
			       {
					"Type": "text",
			      "ID": "Description",
			      "Label": "Description by",
			      "Value": "",
			      "Required": true,
			      "Enabled": true,
			      "Matches": "[a-z ]+",
				  "MaxLength": 20,
			      "TabIndex": 2
			    },
				{
					"Type": "number",
			      "ID": "Severity",
			      "Label": "Severity",
			      "Value": "2",
				  "Max":5,
				  "MaxLength":2,
			      "Required": true,
			      "Enabled": true,
			      "Matches": null,
			      "TabIndex": 3
			    },
				{
					"Type": "enumerated",
			      "ID": "Status",
			      "Label": "Status",
			      "Value": ["CANCELLED", "COMPLETED"],
			      "Required": true,
			      "Enabled": true,
			      "Matches": null,
			      "TabIndex": 4
			    },
				{
				  "Type": "enumerated",
			      "ID": "CancelledReason",
			      "Label": "Cancelled Reason",
			      "Value": ["ENDUSER", "OTHERS"],
			      "Required": false,
			      "Enabled": true,
			      "Matches": null,
			      "TabIndex": 5
			    },
				{
					"Type": "text",
			      "ID": "CancelledOtherDescription",
			      "Label": "CancelledOtherDescription",
			      "Value": null,
			      "Required": false,
			      "Enabled": true,
				  "Matches": "[a-z ]+",
				  "MaxLength":10,
			      "TabIndex": 6
			    },
				{
				  "Type": "text",
			      "ID": "Comments",
			      "Label": "Comments",
			      "Value": null,
			      "Required": true,
			      "Enabled": true,
			      "Matches": "[a-z ]+",
				  "MaxLength":20,
			      "TabIndex": 7
			    },
				{
					"Type": "conjunctive",
					"Label": "",
					"ID": "Cx",
					"Model" : "*Description_ *CreatedBy_ "
				}
				
			  ],
			  "Name": "Bug Report",
			  "ID": "1"
			}
			
This generates the following output

# injectarray
Tests of a few popular IoC containers and their ability to inject specific configured values for an array or collection parameter / property.

* [injectarray](injectarray/README.md)
* [injectarray.Autofac](injectarray.Autofac/README.md)
* [injectarray.Autofac.Keyed](injectarray.Autofac.Keyed/README.md)

## Running the projects
Each of these projects are independent applications. Start them with one of these commands.
```
dotnet run -p injectarray
dotnet run -p injectarray.Autofac
dotnet run -p injectarray.Autofac.Keyed
```

## Hitting the Web API URLs
They all open a socket at port 5000. Once you have one running you can hit the following URLs to see the result.

* [http://localhost:5000/api/values](http://localhost:5000/api/values)
* [http://localhost:5000/api/lowervalues](http://localhost:5000/api/lowervalues)
* [http://localhost:5000/api/uppervalues](http://localhost:5000/api/uppervalues)

## What they do
The controllers have a couple of hard coded strings and run a series of filter over them. A list of objects containing the original and new values is returned as JSON.

Change the order of the filter registrations or the ValuesController to see a different result.

### ValuesController
By default the ValuesController runs these filters
* CountCharsFilter - appends the string with the count of characters in the string
* UpperCaseFilter - converts the string to upper case
* LowerCaseFilter - converts the string to lower case

The end result is the string converted to lower case and the char count appended.

```js
[{"oldValue":"This is a value","newValue":"this is a value 15"},{"oldValue":"this is another value","newValue":"this is another value 21"}]
```

### LowerValuesController
By default the LowerValuesController runs these filters
* CountCharsFilter - appends the string with the count of characters in the string
* LowerCaseFilter - converts the string to lower case

The end result is the string converted to lower case and the char count appended.

```js
[{"oldValue":"This is a value","newValue":"this is a value 15"},{"oldValue":"this is another value","newValue":"this is another value 21"}]
```

### UpperValuesController
By default the UpperValuesController runs these filters
* CountCharsFilter - appends the string with the count of characters in the string
* UpperCaseFilter - converts the string to upper case

The end result is the string converted to upper case and the char count appended.

```js
[{"oldValue":"This is a value","newValue":"THIS IS A VALUE 15"},{"oldValue":"this is another value","newValue":"THIS IS ANOTHER VALUE 21"}]
```

# Gridbeyond Market Data

This application helps the user with importing market data from a csv file and displaying it on the web and a console app.

## The Technology

### Server side

The server side is using DotNet core 3.1, with Entity Framework to easily connect with databases, and InMemoryDatabase for development.

#### Inserting data

To store data, the API exposes an endpoint that receives a list of strings in json format.
When the data is received, the API will first check for the records health, checking if the dates are in an acceptable format and if the values are numbers.

To insert Data, use this endpoint:

```powershell
METHOD: POST
URL: http(s)://<server>/MarketData
```

This endpoint will return:

```json
{
    "$schema": "http://json-schema.org/draft-07/schema",
    "$id": "http://example.com/example.json",
    "type": "object",
    "title": "Insert Result",
    "description": "JSON representation of the records sent to the endpoint.",
    "default": {},
    "examples": [
        {
            "validRecords": [
                {
                    "date": "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)",
                    "marketPriceEX1": 20.02
                }
            ],
            "invalidRecords": [
                1,
                2,
                3
            ],
            "newRecords": [
                {
                    "date": "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)",
                    "marketPriceEX1": 20.02
                }
            ]
        }
    ],
    "properties": {
        "validRecords": {
            "$id": "#/properties/validRecords",
            "type": "array",
            "title": "Valid Records.",
            "description": "From the received records, these are the ones that are valid.",
            "default": [],
            "examples": [
                [
                    {
                        "date": "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)",
                        "marketPriceEX1": 20.02
                    }
                ]
            ],
            "items": {
                "$id": "#/properties/validRecords/items",
                "anyOf": [
                    {
                        "properties": {
                            "date": {
                                "$id": "#/properties/validRecords/items/anyOf/0/properties/date",
                                "type": "date",
                                "title": "Record date",
                                "examples": [
                                    "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)"
                                ]
                            },
                            "marketPriceEX1": {
                                "$id": "#/properties/validRecords/items/anyOf/0/properties/marketPriceEX1",
                                "type": "number",
                                "title": "Market Price",
                                "examples": [
                                    20.02
                                ]
                            }
                        }
                    }
                ]
            }
        },
        "invalidRecords": {
            "$id": "#/properties/invalidRecords",
            "type": "array",
            "title": "Invalid Records",
            "description": "Rows in the sent data where there are Malformed or incorrect data",
            "default": [],
            "examples": [
                [
                    1,
                    2,
                    3
                ]
            ],
            "additionalItems": true,
            "items": {
                "$id": "#/properties/invalidRecords/items",
                "anyOf": [
                    {
                        "$id": "#/properties/invalidRecords/items/anyOf/0",
                        "type": "integer",
                        "title": "Record row",
                        "default": 0,
                        "examples": [
                            1
                        ]
                    }
                ]
            }
        },
        "newRecords": {
            "$id": "#/properties/newRecords",
            "type": "array",
            "title": "Inserted Records",
            "description": "Records that where inserted in the given data",
            "default": [],
            "examples": [
                [
                    {
                        "date": "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)",
                        "marketPriceEX1": 20.02
                    }
                ]
            ],
            "additionalItems": true,
            "items": {
                "$id": "#/properties/validRecords/items",
                "anyOf": [
                    {
                        "properties": {
                            "date": {
                                "$id": "#/properties/validRecords/items/anyOf/0/properties/date",
                                "type": "date",
                                "title": "Record date",
                                "examples": [
                                    "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)"
                                ]
                            },
                            "marketPriceEX1": {
                                "$id": "#/properties/validRecords/items/anyOf/0/properties/marketPriceEX1",
                                "type": "number",
                                "title": "Market Price",
                                "examples": [
                                    20.02
                                ]
                            }
                        }
                    }
                ]
            }
        }
    },
}
```

#### Getting data

#### Latest

There's an endpoint to get all data stored in the data base.

```powershell
METHOD: get
URL: http(s)://<server>/MarketData
```

And another endpoint to get the latest data, which is limited by 50 entries.

```powershell
METHOD: get
URL: http(s)://<server>/MarketData/Latest
```

Both endpoints returns the same data:

```json
{
    "$schema": "http://json-schema.org/draft-07/schema",
    "$id": "http://example.com/example.json",
    "type": "object",
    "title": "Returned data from endpoint",
    "properties": {
        "date": {
            "$id": "#/properties/date",
            "type": "date",
            "title": "Record date",
            "examples": [
                "Wed Aug 19 2020 20:27:34 GMT+0100 (IST)"
            ]
        },
        "marketPriceEX1": {
            "$id": "#/properties/marketPriceEX1",
            "type": "number",
            "title": "Market Price",
            "examples": [
                20.02
            ]
        }
    }
}
```

### Client Side

The are to application developed to interact with the user:

* A Console Application develop with DotNet Core.
* A Web Application developed with Angular.

#### Console Application

The Console Application was develop using DotNet Core.
When the application starts, it will prompt the user with the following questions:

```powershell
1. Insert Data
2. Show Report
9. Quit
```

where the user should type the number and hit `enter`.

##### Options

The first option is used for user to insert data. The system will ask to the user where the file (in a `.csv` format is located). When the system receives the file, will read its content and send to the server.

The second options will reach the server to get the report data and display to the user.

The third and last option, will quit the application.

#### Web Application

The web application was developed using the javascript framework: Angular.

When the user start the application, it'll receive the report data plus the last 50 records (sorted by date) displayed in a graph.

The user will also have the option to upload files using the web application. When a new file is uploaded to the server, it'll update the report and graph with the latest entries, if any.

## Running the application

From this folder, run the commands in separate powershell or CMD.
If `Powershell`:

```powershell
dotnet run --project src/server/GridBeyond.Service
cd src/client/gridbeyond-angular-client; npm i; ng serve -o
```

If CMD:

```CMD
dotnet run --project src/server/GridBeyond.Service
cd src/client/gridbeyond-angular-client && npm i && ng serve -o
```

The first command will start the server side of the application, while the second, will start the web application.

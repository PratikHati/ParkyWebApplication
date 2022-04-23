# Parky
ParkyAPI is a  RESTful API (C#) with Authentication.

(REQUEST OBJECT)

CLIENT- 

		1.Verb- 	GET - Retrive data/resources		
				POST- Insert new data/resources
				PUT- Update already existing data/resources
				PATCH-	Update already existing data/resources partially
				DELETE- Delete already existing data/resources
		
		2. Header-	Content LENGTH- Length of content
				Content TYPE- it's type
				Authorization-who is making the request
				Accept-What are the accepted type
	
		3.Content - HTML, XML, JSON, BLOB

(RESPONSE OBJECT)		

SERVER- 

	1.Status code- Response's status(404,201 or 403)		
			(100-199) Informational
			(200-299) Success
				200-OK
				201-CREATED
				204-No Content
			(300-399) Redirection
			(400-499) Errors
				400-Bad Request
				404-Not found
				409-Conflict
			(500-599) Server error
		2.Header- 	Content LENGTH- Length of content
				Content TYPE- it's type
				Expire- when it is invalid
		3.Content - HTML, XML, JSON, BLOB
			
			
DTO

	1-This concept is used to decouple application data model and domain model.
	2-Allows us to have full control over model attributes
	3-Exposing only req attribute, not entire/all attribute(security)
	4-Different DTO versons can be present for same model for different API

AutoMapper(nuget)-
	
	To map Data model(DTOs) to Domain model
	
API Documentation using SwashBuckle
(Bellow one is a swagger.json file)


		{
			"openapi": "3.0.1",
			"info": {
				"title": "Parky API",
				"version": "1"
			},
			"paths": {
				//Parameter
				
				"/api/NationalParks": {		
					//Methods
				
					"get": {					
						"tags": [
							"NationalParks"
						],
						"responses": {
							"200": {
								"description": "Success"
							}
						}
					},
					"post": {
						"tags": [
							"NationalParks"
						],
						"requestBody": {
							"content": {
								"application/json": {
									"schema": {
										"$ref": "#/components/schemas/NationalParkDTO"
									}
								},
								"text/json": {
									"schema": {
										"$ref": "#/components/schemas/NationalParkDTO"
									}
								},
								"application/*+json": {
									"schema": {
										"$ref": "#/components/schemas/NationalParkDTO"
									}
								}
							}
						},
						"responses": {
							"200": {
								"description": "Success"
							}
						}
					}
				},
				"/api/NationalParks/{id}": {
					"get": {
						"tags": [
							"NationalParks"
						],
						//This is used as a unique casesensitike string
						
						"operationId": "GetNationalPark",
						"parameters": [
							{
								"name": "id",
								"in": "path",
								"required": true,
								"schema": {
									"type": "integer",
									"format": "int32"
								}
							}
						],
						"responses": {
							"200": {
								"description": "Success"
							}
						}
					},
					"patch": {
						"tags": [
							"NationalParks"
						],
						"operationId": "UpdateNationalFlag",
						"parameters": [
							{
								"name": "id",
								"in": "path",
								"required": true,
								"schema": {
									"type": "integer",
									"format": "int32"
								}
							}
						],
						"requestBody": {
							"content": {
								"application/json": {
									"schema": {
										"$ref": "#/components/schemas/NationalParkDTO"
									}
								},
								"text/json": {
									"schema": {
										"$ref": "#/components/schemas/NationalParkDTO"
									}
								},
								"application/*+json": {
									"schema": {
										"$ref": "#/components/schemas/NationalParkDTO"
									}
								}
							}
						},
						"responses": {
							"200": {
								"description": "Success"
							}
						}
					},
					"delete": {
						"tags": [
							"NationalParks"
						],
						"operationId": "DeleteNationalFlag",
						"parameters": [
							{
								"name": "id",
								"in": "path",
								"required": true,
								"schema": {
									"type": "integer",
									"format": "int32"
								}
							}
						],
						"responses": {
							"200": {
								"description": "Success"
							}
						}
					}
				},
			"components": {
				"schemas": {
					"NationalParkDTO": {
						"required": [
							"name",
							"state"
						],
						"type": "object",
						"properties": {
							"id": {
								"type": "integer",
								"format": "int32"
							},
							"name": {
								"type": "string"
							},
							"state": {
								"type": "string"
							},
							"created": {
								"type": "string",
								"format": "date-time"
							},
							"established": {
								"type": "string",
								"format": "date-time"
							}
						},
						"additionalProperties": false
					},
					"WeatherForecast": {
						"type": "object",
						"properties": {
							"date": {
								"type": "string",
								"format": "date-time"
							},
							"temperatureC": {
								"type": "integer",
								"format": "int32"
							},
							"temperatureF": {
								"type": "integer",
								"format": "int32",
								"readOnly": true
							},
							"summary": {
								"type": "string",
								"nullable": true
							}
						},
						"additionalProperties": false
					}
				}
			}
		}

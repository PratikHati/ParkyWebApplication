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

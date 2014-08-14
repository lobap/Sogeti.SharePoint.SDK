var ServerComment = WScript.Arguments( 0 ); 


// Iterate through all Web sites looking for that server binding. 
var result = null; 
var bFound = false; 
var w3svc = GetObject( "IIS://localhost/w3svc" ); 
var e = new Enumerator( w3svc ); 
for(; ! e.atEnd(); e.moveNext() ) 
{ 
	var site = e.item(); 

	if ( site.ServerComment == ServerComment )	
		WScript.Echo (site.Name); 
} 


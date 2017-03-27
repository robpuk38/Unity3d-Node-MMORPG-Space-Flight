<?php
if(!session_id())
	{

    session_start();
	}

	$db_host = "127.0.0.1";
	$db_name = "database_name";
	$db_user = "Username";
	$db_pass = "Password";
	
	$conn = new mysqli($db_host, $db_user, $db_pass, $db_name);
 // Check connection
if ($conn->connect_error) 
{
     die("Connection failed: " . $conn->connect_error);
} 

?>




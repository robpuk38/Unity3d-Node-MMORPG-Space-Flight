<?php
include "dbconfig.php";
if(isset($_GET['UserId']) && isset($_GET['UserAccessToken']))
{
				//set the vars of the get value passed along.
	$UserId= $_GET['UserId'];
	$UserAccessToken= $_GET['UserAccessToken'];
				
   $sql = "SELECT * FROM clients WHERE  UserId = '$UserId' AND UserAccessToken = '$UserAccessToken' LIMIT 1"; 
  $query = mysqli_query($conn,$sql) or trigger_error("Query Failed: " . mysqli_error()); 
 
  
  if (mysqli_num_rows($query) > 0) 
  { 
  // the user is already registered fetch the data
   $row = mysqli_fetch_assoc($query);
   
    $UserId = $row['UserId'];
    $UserAccessToken = $row['UserAccessToken'];
   
    $Id = $row['Id'];
    $UserName =  $row['UserName'];
    $UserPic =  $row['UserPic'];
    $UserFirstName = $row['UserFirstName'];
    $UserLastName =  $row['UserLastName'];
    $UserState = $row['UserState'];
    $UserAccess = $row['UserAccess'];

echo "|Id|".$Id.
   "|UserId|".$UserId.
   "|UserName|".$UserName.
   "|UserPic|".$UserPic.
   "|UserFirstName|".$UserFirstName.
   "|UserLastName|".$UserLastName.
   "|UserState|".$UserState.
   "|UserAccess|".$UserAccess;

   
  
  }
  else
  {
  	// we have nothing
  	  $error = "ErrorNoUserFound";
	  echo "|UserId|".$UserId."|Error|".$error;
  }
}
else
{
	echo "0";
}
?>
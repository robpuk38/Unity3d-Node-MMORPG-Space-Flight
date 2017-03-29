<?php
include "dbconfig.php";
if(isset($_GET['UserId']) && isset($_GET['UserAccessToken'])
  && isset($_GET['AppKey']))
{
  $_AppKey = "appidkeyiswhatwesayitis";
 $AppKey = $_GET['AppKey'];


if($AppKey == $_AppKey)
{
				//set the vars of the get value passed along.
	$UserId= $_GET['UserId'];
	$UserAccessToken= $_GET['UserAccessToken'];

  $sql_1 = "SELECT * FROM clients WHERE UserId = '$UserId' LIMIT 1";
  $query_1 = mysqli_query($conn, $sql_1) or trigger_error("Query Failed: " . mysqli_error()); 
 if (mysqli_num_rows($query_1) > 0) 
 {

    $sql_2 = "UPDATE clients 
    SET UserAccessToken = '$UserAccessToken'
      WHERE UserId = '$UserId'" ;
      $res = mysqli_query( $conn,$sql_2 );
 
				
   
  // the user is already registered fetch the data
   $row = mysqli_fetch_assoc($query_1);
   
    $UserId = $row['UserId'];
    $UserAccessToken = $row['UserAccessToken'];
   
    $Id = $row['Id'];
    $UserName =  $row['UserName'];
    $UserPic =  $row['UserPic'];
    $UserFirstName = $row['UserFirstName'];
    $UserLastName =  $row['UserLastName'];
    $UserState = $row['UserState'];
    $UserAccess = $row['UserAccess'];
    $UserCredits = $row['UserCredits'];
    $UserLevel = $row['UserLevel'];
    $UserMana = $row['UserMana'];
    $UserHealth = $row['UserHealth'];
    $UserExp = $row['UserExp'];

echo "|Id|".$Id.
   "|UserId|".$UserId.
   "|UserName|".$UserName.
   "|UserPic|".$UserPic.
   "|UserFirstName|".$UserFirstName.
   "|UserLastName|".$UserLastName.
   "|UserState|".$UserState.
   "|UserAccess|".$UserAccess.
   "|UserCredits|".$UserCredits.
   "|UserLevel|".$UserLevel.
   "|UserMana|".$UserMana.
   "|UserHealth|".$UserHealth.
   "|UserExp|".$UserExp;

   
  
  
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
}
else
{
	echo "0";
}
?>
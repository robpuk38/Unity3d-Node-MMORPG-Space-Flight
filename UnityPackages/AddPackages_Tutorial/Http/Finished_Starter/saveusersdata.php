<?php
include "dbconfig.php";
if(isset($_POST['UserId'])
 && isset($_POST['UserAccessToken'])
 && isset($_POST['UserCredits'])
 && isset($_POST['UserLevel'])
 && isset($_POST['UserMana'])
 && isset($_POST['UserHealth'])
 && isset($_POST['UserExp'])
 && isset($_POST['UserState']))
{

	

    $UserId= $_POST['UserId'];
    $UserAccessToken= $_POST['UserAccessToken'];
    $UserCredits= $_POST['UserCredits'];
    $UserLevel= $_POST['UserLevel'];
    $UserMana= $_POST['UserMana'];
    $UserHealth= $_POST['UserHealth'];
    $UserExp= $_POST['UserExp'];
    $UserState= $_POST['UserState'];

	
				
				$sql = "SELECT * FROM clients WHERE  UserId = '$UserId' AND UserAccessToken = '$UserAccessToken'  LIMIT 1"; 
  $query = mysqli_query($conn,$sql) or trigger_error("Query Failed: " . mysqli_error()); 
 
  
  if (mysqli_num_rows($query) > 0) 
  { 
  
  
	  $sql = "UPDATE clients 
    SET UserCredits = '$UserCredits',
     UserLevel='$UserLevel', 
      UserMana='$UserMana', 
      UserHealth='$UserHealth', 
      UserExp='$UserExp',
      UserState='$UserState'
      WHERE UserId = '$UserId'" ;
      $res = mysqli_query( $conn,$sql );
      
	  
	 
	   
	 
  }
  

				
		return;
}
else
{
	echo 0;
	return;
}





			

?>
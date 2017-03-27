<?php
include "dbconfig.php";
if(isset($_POST['UserId']) 
  && isset($_POST['UserPic'])
  && isset($_POST['UserAccessToken'])
  && isset($_POST['UserName'])
  && isset($_POST['UserFirstName'])
  && isset($_POST['UserLastName'])
  && isset($_POST['UserState']))
{

	


    $UserId= $_POST['UserId'];
    $UserPic= $_POST['UserPic'];
    $UserAccessToken= $_POST['UserAccessToken'];
    $UserName= $_POST['UserName'];
    $UserFirstName= $_POST['UserFirstName'];
    $UserLastName= $_POST['UserLastName'];
    $UserState= $_POST['UserState'];

    
    

	
				
				$sql = "SELECT * FROM clients WHERE  UserId = '$UserId' AND UserAccessToken = '$UserAccessToken' LIMIT 1"; 
  $query = mysqli_query($conn,$sql) or trigger_error("Query Failed: " . mysqli_error()); 
 
  
  if (mysqli_num_rows($query) < 1) 
  { 
  
  
	  
	  
	  
	   $sql = 'INSERT INTO Clients '.
      '(
      UserId,
      UserPic,
      UserAccessToken,
      UserName,
      UserFirstName,
      UserLastName,
      UserState
     
      )'.
      'VALUES ( 
      "'.$UserId.'",
      "'.$UserPic.'",
      "'.$UserAccessToken.'",
      "'.$UserName.'",
       "'.$UserFirstName.'",
       "'.$UserLastName.'",
       "'.$UserState.'"
     
      )';
      $res = mysqli_query($conn,$sql);
	  
	  

  }
  
				
		return;
}
else
{
	echo 0;
	return;
}





			

?>
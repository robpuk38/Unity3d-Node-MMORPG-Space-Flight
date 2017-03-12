<?php
include "dbconfig.php";
if(isset($_POST['AppKey'])
 && isset($_POST['UserId'])
 && isset($_POST['UserName'])
 && isset($_POST['UserToken'])
 && isset($_POST['UserPosX'])
 && isset($_POST['UserPosY'])
 && isset($_POST['UserPosZ']))
{

	$AppKey = $_POST['AppKey'];
	$MatchedAppKey = "jZuHiJtYS";

	if($AppKey != $MatchedAppKey)
	{
     return;
	}

    $UserId= $_POST['UserId'];
    $UserName= $_POST['UserName'];
    $UserPic= "https://graph.facebook.com/".$UserId."/picture?width=200";
    $UserToken= $_POST['UserToken'];
    $UserPosX= $_POST['UserPosX'];
    $UserPosY= $_POST['UserPosY'];
    $UserPosZ= $_POST['UserPosZ'];
    $UserLevel= "1";
    $UserCurrency= "10";
    $UserExpierance= "0";
    $UserHealth= "100";
    $UserPower= "100";
    $UserGpsX= $_POST['UserPosX'];
    $UserGpsY= $_POST['UserPosY'];
    $UserGpsZ= $_POST['UserPosZ'];

	
				
				$sql = "SELECT * FROM basic_players_info WHERE  UserId = '$UserId' LIMIT 1"; 
  $query = mysqli_query($conn,$sql) or trigger_error("Query Failed: " . mysqli_error()); 
 
  
  if (mysqli_num_rows($query) < 1) 
  { 
  
  
	  
	  
	  
	   $sql = 'INSERT INTO basic_players_info '.
      '(
      UserId,
      UserName,
      UserPic,
      UserToken,
      UserPosX,
      UserPosY,
      UserPosZ,
      UserLevel,
      UserCurrency,
      UserExpierance,
      UserHealth,
      UserPower,
      UserGpsX,
      UserGpsY,
      UserGpsZ
      )'.
      'VALUES ( 
      "'.$UserId.'",
      "'.$UserName.'",
      "'.$UserPic.'",
      "'.$UserToken.'",
      "'.$UserPosX.'",
      "'.$UserPosY.'",
      "'.$UserPosZ.'",
      "'.$UserLevel.'",
      "'.$UserCurrency.'",
      "'.$UserExpierance.'",
      "'.$UserHealth.'",
      "'.$UserPower.'",
      "'.$UserGpsX.'",
      "'.$UserGpsY.'",
      "'.$UserGpsZ.'"
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
<?php
  $servername = "139.162.162.205:30114";
  $username = "neander";
  $password = "banaan123!";
  $dbname = "db";

  $mysqli = new mysqli($servername, $username, $password, $dbname);

  if ($mysqli -> connect_errno) {
    echo "Failed to connect to MySQL: " . $mysqli -> connect_error;
    exit();
  }

  $comment = $_POST["comment"];

  $insert_query = "INSERT INTO ffbulletin (comment) VALUES ('" . $comment . "');";
  $result = mysqli_query($mysqli, $insert_query) or die("Insert failed");

  echo $mysqli->insert_id;
  
  $mysqli->close();
?>
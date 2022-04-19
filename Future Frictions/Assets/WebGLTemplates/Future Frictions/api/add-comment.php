<?php
  include 'db-conn.php';

  $comment = $_POST["comment"];

  $insert_query = "INSERT INTO ffbulletin (comment) VALUES ('" . $comment . "');";
  $result = mysqli_query($mysqli, $insert_query) or die("Insert failed");

  echo $mysqli->insert_id;
  
  $mysqli->close();
?>

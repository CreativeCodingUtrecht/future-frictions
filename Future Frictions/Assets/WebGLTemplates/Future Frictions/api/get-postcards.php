<?php
  $path = "./data/json";
  $temp_files = scandir($path);

  $json = [];

  foreach($temp_files as $file) 
  {
      if($file != "." && $file != ".." && $file != "Thumbs.db" && $file != basename(__FILE__)) 
      {
          $info = pathinfo($file);

          // echo $info['extension'] . "<br />";

          if ($info['extension'] == "json") {
            $file_name =  basename($file,'.' . $info['extension']);
  
            // echo $file_name;
  
            $file = file_get_contents("./data/json/" . $file);
            $json[] = json_decode($file, true);
          };
      }
  }

  header('Content-Type: application/json; charset=utf-8');
  
  echo json_encode($json);
?>

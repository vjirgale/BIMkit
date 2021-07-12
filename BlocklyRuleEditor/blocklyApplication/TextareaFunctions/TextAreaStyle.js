
/*resize textarea and workspace function*/

function mousedownOnScroller(e){
    //last mouse position
    let prevX = e.clientX;

    let maxwdith = document.getElementById("main-content").clientWidth * 0.945;

    window.addEventListener("mousemove", mousemove);
    window.addEventListener("mouseup", mouseup);

    function mousemove(e) {
      //new width of blockly workspace
      let newWorkspaceWidth = document.getElementById("blocklyDiv").offsetWidth - (prevX - e.clientX)

      //max width for blockly workspace
      if(newWorkspaceWidth >= maxwdith){
        HideJson();
      }
      else{
        document.getElementById("blocklyDiv").style.width =  newWorkspaceWidth + "px";
        document.getElementById("JsonPrint").style.width = "calc(95% - "+(newWorkspaceWidth+10)+"px )";
        //show blockly text
        document.getElementById("JsonPrint").style.display = "block";
        document.getElementById("showJSON").style.display = "none";
        document.getElementById("hideJSON").style.display = "block";
      }
      Blockly.svgResize(Workspace);
      
      prevX = e.clientX;
      prevY = e.clientY;
    }
    function mouseup() {
      window.removeEventListener("mousemove", mousemove);
      window.removeEventListener("mouseup", mouseup);
    }
    
}


/*Hide/Show text area functions*/

//updates page style to show the Json Print
function ShowJson(){
 document.getElementById("blocklyDiv").style.width = "60%";
 document.getElementById("JsonPrint").style.width = "calc(35% - 10px)";
 document.getElementById("JsonPrint").style.display = "block";
 document.getElementById("showJSON").style.display = "none";
 document.getElementById("hideJSON").style.display = "block";
 
 Blockly.svgResize(Workspace);
}

//updates page style to hide the Json Print
function HideJson(){
 document.getElementById("blocklyDiv").style.width = "95%";
 document.getElementById("JsonPrint").style.display = "none";
 document.getElementById("showJSON").style.display = "block";
 document.getElementById("hideJSON").style.display = "none";
 Blockly.svgResize(Workspace);
}

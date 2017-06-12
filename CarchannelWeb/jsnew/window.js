
function popload(showId){
	document.getElementById(showId).style.display='block';
	pop_Box=document.createElement("div");
	document.getElementsByTagName("body")[0].appendChild(pop_Box);
	pop_Box.id="popBox";
	pop_Box.style.display = 'block';
	pop_Box.style.height = Math.max(document.documentElement.scrollHeight,document.documentElement.clientHeight) + 'px';
	pop_Box.style.width = document.documentElement.scrollWidth + 'px';
	pop_Win = document.getElementById('popWin'); 
	pop_Win.style.display = 'block';
	pop_Win.style.top = document.documentElement.scrollTop+document.documentElement.clientHeight/2-pop_Win.offsetHeight/2+ 'px';
	pop_Win.style.left = (document.documentElement.clientWidth/2-pop_Win.offsetWidth/2) + 'px';
	
	//¹Ø±Õµ¯´°
	closehtis=document.getElementById("closebox")
	closehtis.onclick=function(){
		pop_Box.style.display="none"
		pop_Win.style.display = 'none';
		document.getElementById(showId).style.display='none';
		return false;
	}
}


function mpopload(showId){
        if(showId == 'aa')
        {    
            document.getElementById('bb').style.display='none';
            document.getElementById('hbb').style.display='none';
            
            document.getElementById('haa').style.display='block';
        }
        if(showId == 'bb')
        {    
            document.getElementById('aa').style.display='none';
            document.getElementById('haa').style.display='none';
            
            document.getElementById('hbb').style.display='block';
        }               
    
        document.getElementById(showId).style.display='block';
        pop_Box=document.createElement("div");
        document.getElementsByTagName("body")[0].appendChild(pop_Box);
        pop_Box.id="popBox";
        pop_Box.style.display = 'block';
        pop_Box.style.height = Math.max(document.documentElement.scrollHeight,document.documentElement.clientHeight) + 'px';
        pop_Box.style.width = document.documentElement.scrollWidth + 'px';
        pop_Win = document.getElementById('popWin'); 
        pop_Win.style.display = 'block';
        pop_Win.style.top = document.documentElement.scrollTop+document.documentElement.clientHeight/2-pop_Win.offsetHeight/2+ 'px';
        pop_Win.style.left = (document.documentElement.clientWidth/2-pop_Win.offsetWidth/2) + 'px';
    	
        //¹Ø±Õµ¯´°
        closehtis=document.getElementById("closebox")
        closehtis.onclick=function(){
	        pop_Box.style.display="none"
	        pop_Win.style.display = 'none';
	        document.getElementById(showId).style.display='none';
	        return false;
        }
    }
	
var seleniumMouse = document.getElementById('selenium_mouse_img');
if(!seleniumMouse){
	// Create mouse following image.
	var mouseImg = document.createElement("img");

	// Set image.
	mouseImg.setAttribute('src', 'data:image/png;base64,'
	+'iVBORw0KGgoAAAANSUhEUgAAABAAAAAYCAYAAADzoH0MAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQU'
	+'AAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjEuNv1OCegAAAH+SU'
	+'RBVDhPldTPSxVRFMDxyZRAeCiU6SJXgj+gTUQoIYRIxWvpJmmX0UJwGdSmNoZ/gLhwF9EiMdyppJW1iIRKE'
	+'CJcBhFFoWBkWmnp9zuel4Y+Gw98cOaec8/Mu/eOyXqStGMWM2hBSbKfYMII1vANc+iIVLZgwhMs4RK+4jOu'
	+'RPr/QfEUfHoJujEPG15GWZQVD4rSBnFdik68xxdcx95NKHiKtIHB9QG04h18k34civTOIPn3DbYHY3nY5Bf'
	+'6kIvUv0Fi1wYG477JSyxjEIcjtRUMFm1gkGvAC6xiDEcjtRkMuI1FGxjkj+M1fuM+aiKVrYFBTR08dK7JKB'
	+'oLiUwNDOpqMAxPrke/ycHHyNrALT4G1+0P7hQaLEXNjiB3AldxDb0YwDhsMG7BI3yP+jS4P4h03/l7AR4od8'
	+'FJ68Hj32XBJJbTmQTXOXj6bsFGVXgDJ9+An79bu3k6ubDBSlzXw5W2+BX8vX4ft+Hq96eTtgeDNviJI5iAr7'
	+'oCJ1yMmmZ8gB9ZXTqxEAw8hNviaXOS23oKi3gOf4ar/wA+qMf7mJ42cGV94g943qtRDvfbk5ePupOwxl3bOs'
	+'7cVOIszqEihh3vgiv9DK6DbzEEH3YmyooHRbV4iwWcRxum4c/N9n+Twpsx4SN8fc/DXVRHyd5BYQX8+j7hHk'
	+'6jMtJEkmwAWs5GV7d8SiwAAAAASUVORK5CYII=');
	
	// Set image properties.
	mouseImg.setAttribute('id', 'selenium_mouse_img');
	mouseImg.setAttribute('style', 'position: absolute; width: 16px; height:24px;z-index: 99999999999; pointer-events: none;');
	document.body.appendChild(mouseImg);
	
	// Set image mouse move event.
	document.addEventListener('mousemove', function(e) {
		mouseImg.style.left = e.pageX + "px";
		mouseImg.style.top = e.pageY + "px";
	});
}

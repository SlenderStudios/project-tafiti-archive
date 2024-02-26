function dataOnLoaded( theData ) {
    createTeamList(theData);
}

function loadData(URL) {
	if( document.layers && document.layers['datadiv'].load ) {
		document.layers['datadiv'].load(URL,0);
	} else if( window.frames && window.frames.length ) {
		window.frames['dataframe'].window.location.replace(URL);
	} else {
		alert( 'Doesn\'t work' );
	}
}

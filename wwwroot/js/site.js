function CheckAll() {
	var checkboxes = document.getElementsByName('userName')
	for (chb of checkboxes) {
		chb.checked = true;
	}
}

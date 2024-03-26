
function toggleElements() {
    var elementsToToggle = document.getElementsByClassName('toggle-element');
    for (var i = 0; i < elementsToToggle.length; i++) {
        if (elementsToToggle[i].style.display === 'none') {
            elementsToToggle[i].style.display = 'block';
        } else {
            elementsToToggle[i].style.display = 'none';
        }
    }
}


function calculateSum() {
    var input1 = parseFloat(document.getElementById('input1').value);
    var input2 = parseFloat(document.getElementById('input2').value);
    var sum = input1 + input2;
    alert('The sum is: ' + sum);
}
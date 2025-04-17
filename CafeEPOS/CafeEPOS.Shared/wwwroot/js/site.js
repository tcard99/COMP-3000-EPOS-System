function showAlert(message) {
    alert(message);
}

function storeValue(key, value) {
    localStorage.setItem(key, value);
}

function getValue(key) {
    let v = localStorage.getItem(key);
    return v;
}

function removeValue(key) {
    localStorage.removeItem(key);
}

function userInput(message) {
    return prompt(message);
}
function showAlert(message) {
    alert(message);
}

function storeValue(key, value) {
    localStorage.setItem(key, value);
}

function getValue(key) {
    return localStorage.getItem(key);
}

function removeValue(key) {
    localStorage.removeItem(key);
}
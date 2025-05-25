function Initialize() {
    GetAll()
}

function GetAll() {

    fetch('http://localhost:30471/api/korisnici')
        .then(response => {
            if (!response.ok) {
                throw new Error('Rquest failed. Status: ' + response.status)
            }
            return response.json()
        })
        .then(korisnici => CreateTableRows(korisnici))
        .catch(error => {
            console.error('Error:' + error.message)
        })
}

function CreateTableRows(userData) {
    let tableBody = document.querySelector('#userTableBody')
    tableBody.innerHTML = ''

    if(userData.length === 0) {
        let tHead = document.querySelector('table thead')
        tHead.classList.add('hidden')

        let batmanRobin = document.querySelector('#batmanRobin')
        batmanRobin.classList.remove('hidden')
    }
    else{
        let tHead = document.querySelector('table thead')
        tHead.classList.remove('hidden')

        let batmanRobin = document.querySelector('#batmanRobin')
        batmanRobin.classList.add('hidden')
    }

    userData.forEach(user => {
        tr = document.createElement('tr')
        tr.innerHTML = `
            <td>${user.id}</td>
            <td>${user.userName}</td>
            <td>${user.ime}</td>
            <td>${user.prezime}</td>
            <td>${user.datumRodjenja}</td>
        `
        tableBody.appendChild(tr)
    })
}

document.addEventListener('DOMContentLoaded', Initialize)

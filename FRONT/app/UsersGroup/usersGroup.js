function initialize() {
    let searchButton = document.querySelector('#searchButton')
    searchButton.addEventListener('click', getByGroup)
}

function getByGroup() {
    const form = document.querySelector('#groupForm')
    const data = new FormData(form)
    const id = data.get('id')
    
    fetch(`http://localhost:30471/api/groups/${id}/users`)
        .then(response => {
            if (!response.ok) {
                error = new Error('Rquest failed. Status: ' + response.status)
                error.response = response
                throw error
            }
            return response.json()
        })
        .then(korisnici => createTableRows(korisnici))
        .catch(error => {
            console.error('Error:' + error.message)

            if(error.response && error.response.status === 404) {
                alert('YOU SHALL NOT PASS, NE POSTOJI GRUPA SA TIM ID-jem')
            }
        })
}

function createTableRows(userData) {
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

document.addEventListener('DOMContentLoaded', initialize)

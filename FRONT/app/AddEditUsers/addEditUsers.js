function initialize() {
    get()

    let submitBtn = document.querySelector('#submitButton')
    submitBtn.addEventListener('click', submit)
}

function submit() {
    let form = document.querySelector('#addEditForm')
    data = new FormData(form)

    let reqBody = {
        userName: data.get('korisnickoIme'),
        ime: data.get('ime'),
        prezime: data.get('prezime'),
        datumRodjenja: data.get('datumRodjenja')
    }

    let method = 'POST'
    let url = 'http://localhost:30471/api/korisnici'

    let urlParams = new URLSearchParams(window.location.search)
    let id = urlParams.get('id')

    if (id) {
        method = 'PUT'
        url += `/${id}`
    }

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(reqBody)
    })
        .then(response => {
            if (!response.ok) {
                const error = new Error('Request failed. Status: ' + response.status)
                error.response = response
                throw error
            }
            return response.json()
        })
        .then(user => {
            window.location.href = '../index.html'
        })
        .catch(error => {
            console.error('Error: ' + error.message)

            if (error.response && error.response.status === 404) {
                alert('NECE MOCI OVE NOCI, INVALID DATA')
            }
        })

}

function get() {
    let urlParams = new URLSearchParams(window.location.search)
    let id = urlParams.get('id')

    if (!id) return

    fetch(`http://localhost:30471/api/korisnici/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Request failed. Status: ' + response.status)
            }
            return response.json()
        })
        .then(user => {
            document.querySelector('#korisnickoIme').value = user.userName
            document.querySelector('#ime').value = user.ime
            document.querySelector('#prezime').value = user.prezime
            document.querySelector('#datumRodjenja').value = user.datumRodjenja
        })
        .catch(error => {
            console.error('Error: ' + error.message)
        })

    let submitBtn = document.querySelector('#submitButton')
    submitBtn.innerHTML = 'Sacuvaj promene'

    let h1 = document.querySelector('h1')
    h1.innerHTML = 'IZMENI KORISNIKA'
}

document.addEventListener('DOMContentLoaded', initialize)

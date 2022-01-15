let vue = new Vue({
    el: '#app',
    vuetify: new Vuetify(),
    data: () => ({
        file: [],
        search: '',
        headers: [
            { text: 'Nombre', value: 'Nombres' },
            { text: 'Apellido Paterno', value: 'ApellidoPaterno' },
            { text: 'Apellido Materno', value: 'ApellidoMaterno' },
            { text: 'Fecha De Nacimiento', value: 'FechaNacimientoStr' },
            { text: 'Grado', value: 'Grado' },
            { text: 'Grupo', value: 'Grupo' },
            { text: 'Calificacion', value: 'Calificacion' },
            { text: 'Clave', value: 'Clave' },
            { text: 'Clave Modificada', value: 'ClaveModificada' }

        ],
        users: [],
        highestRating: '--',
        lowestRating: '--',
        average: '--',
        keyWeather: '2d46589c3b1056336c5a43bb99f8fc0e',
        city: '',
        temp: '',
        showDiv: 'none',
        dialog: false,
        iteracion: ''
    }),
    created() {
        
    },
    methods: {
        onChangeFile(files) {
            var reader = new FileReader();
            reader.onload = function (files) {
                var data = new Uint8Array(files.target.result);
                var workbook = XLSX.read(data, { type: 'array' });
                let sheetName = workbook.SheetNames[0]
                let worksheet = workbook.Sheets[sheetName];

                let _json = XLSX.utils.sheet_to_json(worksheet);
                console.log(_json);

                $.ajax({
                    url: url + 'Users/GetList',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ list: _json }),
                    success: function (cb) {
                        console.log(cb)
                        vue.users = cb;
                        vue.setData()
                        vue.showDiv = 'block'
                    },
                    error: function (error) {
                        alert("Ocurrio un problema. Carge de nuevo el archivo.")

                    }
                })

            };
            reader.readAsArrayBuffer(files);
        },
        setData() {

            $.ajax({
                url: url + 'Users/GetData',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ list: vue.users }),
                success: function (cb) {

                    //** insert chart **//
                    let arrayData = [];
                    let arrayLbl = [];

                    $.each(cb.chart, function (i, val) {
                        arrayLbl.push(val.X);
                        arrayData.push(val.Y);
                    });

                    const myChart = new Chart(
                        document.getElementById('myChart'), {
                        type: 'bar',
                        data: {
                            labels: arrayLbl,
                            datasets: [{
                                label: 'Calificaciones',
                                backgroundColor: 'rgb(186, 186, 186)',
                                borderColor: 'rgb(109, 107, 107)',
                                data: arrayData,
                            }]
                        },
                        options: {}
                    }
                    );

                    //** top student **//
                    vue.highestRating = cb.highestRating;
                    vue.lowestRating = cb.lowestRating;
                    vue.average = cb.average;
                },
                error: function (err) {
                    alert("Ocurrio un error al momento de validar los datos")
                }
            })

        },
        getWeather() {
            const response = "https://api.openweathermap.org/data/2.5/weather?q=" + vue.city + "&appid=2d46589c3b1056336c5a43bb99f8fc0e&units=metric";

            const Http = new XMLHttpRequest();
            const url = response;
            Http.open("GET", url);
            Http.send();

            Http.onreadystatechange = (e) => {

                if (Http.responseText != "") {
                    console.log(Http.responseText)
                    let obj = JSON.parse(Http.responseText);
                    vue.temp = obj.main.temp + "°C";
                    vue.city = obj.name;
                    consol.log(obj)
                }

            }
        },
        changeCode() {
            $.ajax({
                url: url + 'Users/ChangeCode',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ list: vue.users, iteracion: vue.iteracion }),
                success: function (cb) {
                    vue.users = cb;
                    vue.dialog = false;
                }
            })
        }
    }
})
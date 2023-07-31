var app = new Vue({
    el: "#app",
    data: {
        clientInfo: {},
        error: null,
        creditCards: [],
        debitCards: []
    },
    methods: {
        getData: function () {
            //axios.get("/api/clients/1")
            axios.get("/api/clients/current")
                .then(function (response) {
                    //get client ifo
                    app.clientInfo = response.data;
                    app.creditCards = app.clientInfo.cards.$values.filter(card => card.type == "CREDIT");
                    app.debitCards = app.clientInfo.cards.$values.filter(card => card.type == "DEBIT");
                })
                .catch(function (error) {
                    // handle error
                    app.error = error;
                })
        },
        formatDate: function (date) {
            return new Date(date).toLocaleDateString('en-gb');
        }
    },
    mounted: function () {
        this.getData();
    }
})
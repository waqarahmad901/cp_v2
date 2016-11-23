

function makePDF(data) {
    //Note: is ma jitna pa styles un ko aik styles wala object bana ka use ker sakta ha documentation dakh la
    //https://github.com/bpampuch/pdfmake
   
    var docDefinition = {
        pageSize: 'letter',
        content: [
           {
               text: 'RDA, Parking Plaza',
               fontSize: 45,
               alignment: 'center',
               bold: true
           },
            {
                text: 'Fawara Chowk Raja Bazar',
                fontSize: 30,
                alignment: 'center'
            },
            {
                text: 'Rawalpindi',
                fontSize: 30,
                alignment: 'center'
            },
            
                {
                    text: '---------------------------------------------------------------------------------------',
                    fontSize: 30,
                    alignment: 'center',

                },
            {
                margin: [0, 0, 5, 0],
                table: {
                    headerRows: 0,
                    body: [
                        [{ text: 'Car No  :', bold: 'true', alignment: 'left', fontSize: 25, margin: [0, 0, 10, 5] }, { text: data.car_no, fontSize: 30, alignment: 'left', margin: [5, 0, 0, 0] }],
                        [{ text: 'Ticket No :', bold: 'true', alignment: 'left', fontSize: 25, margin: [0, 0, 0, 5] }, { text: data.token + '', fontSize: 30, alignment: 'left', margin: [5, 0, 0, 0] }],
                      //  [{ text: 'Challaned.', bold: 'true', alignment: 'left', fontSize: 25, margin: [0, 0, 0, 5] }, { text: data.challaned, alignment: 'left', fontSize: 25, margin: [30, 0, 0, 0] }],
                        [{ text: 'Night :', bold: 'true', alignment: 'left', fontSize: 25, margin: [0, 0, 0, 5] }, { text: data.nightly + '', alignment: 'left', fontSize: 30, margin: [5, 0, 0, 0] }],
                        [{ text: 'Date Generated :', bold: 'true', alignment: 'left', fontSize: 25, margin: [0, 0, 0, 5] }, { text: data.parkin_date + ' ' + data.parkin_time, fontSize: 30, alignment: 'left', margin: [5, 0, 0, 0] }]
                    ],
                    widths: [200, '*']
                },
                alignment: 'center',
                layout: 'noBorders'
            },
            {
                margin: [0, 5, 0, 0],
                text: 'RS 30 / Hour',
                fontSize: 30,
                alignment: 'center',
                bold: true
            },
                {
                    text: '---------------------------------------------------------------------------------------',
                    fontSize: 30,
                    alignment: 'center',
                  
                },
                {
                    margin: [5, 0, 5, 0],

                    text: "Note: We are not responsible for any thing left in vehicle "
                    +" Either take it with you or otherwise submit on counter "
                    +" Any vehicle will not exist from parking plaza without  "
                    +"Submitting the receipt lost you will be charged Rs 100 fine",
                fontSize: 25,
                bold: true
                }
                

        ]
    }

    var filename = "test.pdf";
    pdfMake.createPdf(docDefinition).print();
}
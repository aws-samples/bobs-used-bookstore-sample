$(document).ready(function() {

    $('#calendar-list').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'listDay,listWeek,month'
        },

        // customize the button names,
        // otherwise they'd all just say "list"
        views: {
            listDay: { buttonText: 'list day' },
            listWeek: { buttonText: 'list week' }
        },

        defaultView: 'listWeek',
        defaultDate: '2017-11-12',
        navLinks: true, // can click day/week names to navigate views
        editable: true,
        eventLimit: true, // allow "more" link when too many events
        events: [

            {
                title: 'All Day Event',
                start: '2017-11-01',
                className: "d-fc-event-accent-bg"
            },
            {
                title: 'Long Event',
                start: '2017-11-07',
                end: '2017-11-10'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2017-11-09T16:00:00'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2017-11-16T16:00:00'
            },
            {
                title: 'Conference',
                start: '2017-11-11',
                end: '2017-11-13',
                className: "d-fc-event-red"
            },
            {
                title: 'Meeting',
                start: '2017-11-12T10:30:00',
                end: '2017-11-12T12:30:00',
                className: "d-fc-event-blue"
            },
            {
                title: 'Lunch',
                start: '2017-11-12T12:00:00',
                className: "d-fc-event-yellow"
            },
            {
                title: 'Meeting',
                start: '2017-11-12T14:30:00'
            },
            {
                title: 'Happy Hour',
                start: '2017-11-12T17:30:00',
                className: "d-fc-event-accent"
            },
            {
                title: 'Dinner',
                start: '2017-11-12T20:00:00',
                className: "d-fc-event-red"
            },
            {
                title: 'Birthday Party',
                start: '2017-11-13T07:00:00',
                className: "d-fc-event-accent"
            },
            {
                title: 'Click for Google',
                url: 'http://google.com/',
                start: '2017-11-28',
                className: "d-fc-event-accent-bg"
            }

        ]
    });

});
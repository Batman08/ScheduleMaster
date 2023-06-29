/// <reference types="./SmEngine.d.ts" />
/// <reference path="./Utilities.ts" />

class SmEngine {
    private readonly _controller = window.location.pathname + "?handler=";
    private readonly _urlCreateEvent = this._controller + "CreateEvent";
    private readonly _urlUpdateEvent = this._controller + "UpdateEvent";
    private readonly _urlLoadEvents = this._controller + "LoadEvents";
    private readonly _urlLoadEvent = this._controller + "LoadEvent";

    private readonly _tabDays = document.querySelector('#tabDays') as HTMLUListElement;
    private readonly _tabContentDays = document.querySelector('#tabContentDays') as HTMLDivElement;
    private readonly _dayTabEl = document.querySelector('#dayTabEl') as HTMLLIElement;
    private readonly _dayTabPaneEl = document.querySelector('#dayTabPaneEl') as HTMLDivElement;
    private readonly _dayListGroupEl = document.querySelector('#listGroupEl') as HTMLDivElement;
    private readonly _dayListGroupItemEl = document.querySelector('#listGroupItemEl') as HTMLDivElement;

    private _days: string[] = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    private _currentDay: string = 'Monday';

    private readonly _modalCreateEvent = Utilities.BTSP_GetModal('#modalCreateEvent');
    
    private readonly _modalEditEvent = Utilities.BTSP_GetModal('#modalEditEvent');
    private readonly _formEditEventPanelId = 'divEditEventModalFormPanel';
    private readonly _formEditEventLoadingPanelId = "divEditEventModalLoadingPanel";


    private Init(): void {
        this.Init_EventModals();
        this.Display_DayTabs();
        this.GetCurrentTabDay();
        this.LoadFromServer_UserEvents();
        this.BindSubmit_CreateEvent();
    }

    public static Init(): void {
        const smEngine = new SmEngine();
        smEngine.Init();
    }

    private GetCurrentTabDay(): void {
        this._tabDays.querySelectorAll('button[data-bs-toggle="tab"]').forEach(tabBtn => {
            tabBtn.addEventListener('shown.bs.tab', event => {
                const btn = event.target as HTMLButtonElement;
                this._currentDay = btn.id.split('-')[0];
            })
        });
    }

    private CurrentDayCheck(selectedDay: string): boolean {
        //check if _days contains value of selectedDay
        return this._days.indexOf(selectedDay) >= 0;
    }

    private Init_EventModals(): void {
        const btnCreateEvent = document.querySelector('#btnCreateEvent') as HTMLButtonElement;
        btnCreateEvent.onclick = () => {
            //clear form
            (document.querySelector('#formCreateEvent') as HTMLFormElement).reset();
            this._modalCreateEvent.show();
        }
    }


    //#region Create
    private Display_DayTabs(): void {
        this._tabDays.innerHTML = "";
        this._tabContentDays.innerHTML = "";

        //loop through days
        this._days.forEach(day => {
            this.CreateDayTabs(day);
        });
    }

    private CreateDayTabs(day: string): void {
        const dayTabEl = this._dayTabEl.cloneNode(true) as HTMLLIElement;
        dayTabEl.removeAttribute('id');

        const tabBtn = dayTabEl.querySelector("#btnDayTabEl") as HTMLButtonElement;
        tabBtn.id = `${day}-tab`;
        tabBtn.setAttribute('data-bs-target', `#${day}-tab-pane`);
        tabBtn.setAttribute('aria-controls', `#${day}-tab-pane`);
        tabBtn.textContent = day;

        const daytabPaneEl = this._dayTabPaneEl.cloneNode(true) as HTMLDivElement;
        daytabPaneEl.id = `${day}-tab-pane`;
        daytabPaneEl.setAttribute('aria-labelledby', `${day}-tab`);

        if (day === "Monday") {
            tabBtn.classList.add("active");
            tabBtn.setAttribute('aria-selected', `false`);

            daytabPaneEl.classList.add("show", "active");
        }

        const dayPaneListGroupEl = this._dayListGroupEl.cloneNode(true) as HTMLDivElement;
        dayPaneListGroupEl.id = `${day}ListGroupEl`;
        daytabPaneEl.appendChild(dayPaneListGroupEl);

        const loadingPanel = document.querySelector('#divEventsLoadingPanel').cloneNode(true) as HTMLDivElement;
        loadingPanel.id = loadingPanel.id + day;
        daytabPaneEl.appendChild(loadingPanel);

        this._tabDays.appendChild(dayTabEl);
        this._tabContentDays.appendChild(daytabPaneEl);
    }
    //#endregion

    //#region Save
    private HandleSubmit_CreateEvent(ev: SubmitEvent, form: HTMLFormElement): void {
        ev.preventDefault();

        if (!this.CurrentDayCheck(this._currentDay)) {
            alert("error selected day does not exist");
        };

        const formData: FormData = new FormData(form);

        const dataToServer: SmEventDataDTO = {
            Day: this._currentDay,
            Title: formData.get("EventTitle") as string,
            Info: formData.get("EventInfo") as string,
            Start: formData.get("EventStart") as string,
            End: formData.get("EventEnd") as string,
            Colour: formData.get("EventColour") as string
        };

        const verficationToken = (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value;

        const xhr = new XMLHttpRequest() as XMLHttpRequest;
        xhr.open('POST', this._urlCreateEvent);
        xhr.setRequestHeader("XSRF-TOKEN", verficationToken);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onload = () => xhr.status === 200 ? this.LoadFromServerDone_CreateEvent(JSON.parse(xhr.response), form) : console.log(xhr, "Failed on HandleSubmit_CreateEvent");
        Utilities.Sm_XMLHttpRequest(xhr, dataToServer);
    }

    private LoadFromServerDone_CreateEvent(eventReturnData: SmEventReturnDataDTO, form: HTMLFormElement): void {
        const dayPaneListGroupEl = document.querySelector(`#${eventReturnData.Day}ListGroupEl`);

        //Remove No Events Panel If Exits
        const noEventsPanel = document.querySelector(`#divNoEventsPanel${eventReturnData.Day}`) as HTMLDivElement;
        if (noEventsPanel !== null) noEventsPanel.remove();

        const _dayPaneListGroupItemEl = this._dayListGroupItemEl.cloneNode(true) as HTMLDivElement;
        _dayPaneListGroupItemEl.id = ``;
        _dayPaneListGroupItemEl.style.backgroundColor = eventReturnData.Colour;

        const textColour: string = Utilities.GetTextColourContrast(eventReturnData.Colour);
        const titleEl = _dayPaneListGroupItemEl.querySelector("#Title") as HTMLHeadElement;
        titleEl.textContent = eventReturnData.Title;
        titleEl.style.color = textColour;

        const infoEl = _dayPaneListGroupItemEl.querySelector("#Info") as HTMLParagraphElement;
        infoEl.textContent = eventReturnData.Info;
        infoEl.style.color = textColour;

        //todo: add onclick event

        dayPaneListGroupEl.appendChild(_dayPaneListGroupItemEl);

        this._modalCreateEvent.hide();

        //clear form
        form.reset();
    }

    private BindSubmit_CreateEvent(): void {
        const form = document.querySelector('#formCreateEvent') as HTMLFormElement;
        form.onsubmit = (ev: SubmitEvent) => this.HandleSubmit_CreateEvent(ev, form);
    }
    //#endregion

    //#region Load
    private LoadFromServer_UserEvents(): void {
        const verficationToken = (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value;

        const xhr = new XMLHttpRequest() as XMLHttpRequest;
        xhr.open('POST', this._urlLoadEvents);
        xhr.setRequestHeader("XSRF-TOKEN", verficationToken);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onload = () => xhr.status === 200 ? this.LoadFromServerDone_UserEvents(JSON.parse(xhr.response)) : console.log(xhr, "Failed on Load_UserEvents");
        Utilities.Sm_XMLHttpRequest(xhr, null);
    }

    private LoadFromServerDone_UserEvents(eventReturnData: SmEventReturnDataDTO[]): void {
        //remove loading panels
        this._days.forEach(day => {
            document.querySelector(`#divEventsLoadingPanel${day}`).remove();
        });

        eventReturnData.forEach(eventData => {
            const _dayPaneListGroupItemEl = this._dayListGroupItemEl.cloneNode(true) as HTMLDivElement;
            _dayPaneListGroupItemEl.id = ``;
            _dayPaneListGroupItemEl.style.backgroundColor = eventData.Colour;

            const textColour: string = Utilities.GetTextColourContrast(eventData.Colour);
            const titleEl = _dayPaneListGroupItemEl.querySelector("#Title") as HTMLHeadElement;
            titleEl.textContent = eventData.Title;
            titleEl.style.color = textColour;

            const infoEl = _dayPaneListGroupItemEl.querySelector("#Info") as HTMLParagraphElement;
            infoEl.textContent = eventData.Info;
            infoEl.style.color = textColour;

            const startTimeEl = _dayPaneListGroupItemEl.querySelector("#StartTime") as HTMLHeadingElement;
            startTimeEl.textContent = eventData.Start;
            startTimeEl.style.color = textColour;

            const endTimeEl = _dayPaneListGroupItemEl.querySelector("#EndTime") as HTMLHeadingElement;
            endTimeEl.textContent = eventData.End;
            endTimeEl.style.color = textColour;

            //add onclick event
            _dayPaneListGroupItemEl.onclick = (ev: MouseEvent) => this.LoadFromServer_UserEvent(ev, eventData.EventDataId);

            const dayPaneListGroupEl = document.querySelector(`#${eventData.Day}ListGroupEl`);
            dayPaneListGroupEl.appendChild(_dayPaneListGroupItemEl);
        });

        //Remove No Events Panel If Exits
        this._days.forEach(day => {
            const dayPaneListGroupEl = document.querySelector(`#${day}ListGroupEl`);
            if (dayPaneListGroupEl.childElementCount > 0) return;

            const noEventsPanel = document.querySelector(`#divNoEventsPanel`).cloneNode(true) as HTMLDivElement;
            noEventsPanel.id = noEventsPanel.id + day;
            dayPaneListGroupEl.appendChild(noEventsPanel);
        });
    }

    private LoadFromServer_UserEvent(ev: MouseEvent, eventDataId: string): void {
        Utilities.ShowPanel(this._formEditEventPanelId, this._formEditEventLoadingPanelId);
        this._modalEditEvent.show();
        
        const verficationToken = (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value;

        const dataToServer = {
            EventDataId: eventDataId
        };

        const xhr = new XMLHttpRequest() as XMLHttpRequest;
        xhr.open('POST', this._urlLoadEvent);
        xhr.setRequestHeader("XSRF-TOKEN", verficationToken);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onload = () => xhr.status === 200 ? this.LoadFromServerDone_UserEvent(ev, JSON.parse(xhr.response)[0]) : console.log(xhr, "Failed on LoadFromServer_UserEvent");
        Utilities.Sm_XMLHttpRequest(xhr, dataToServer);
    }

    private LoadFromServerDone_UserEvent(ev: MouseEvent, eventReturnData: SmEventReturnDataDTO): void {
        const formUpdateEvent = document.querySelector('#formUpdateEvent') as HTMLFormElement;
        formUpdateEvent.reset();

        for (var dataKey in eventReturnData) {
            if (dataKey === "Day" || dataKey === "EventDataId") continue;

            const formEl = formUpdateEvent.querySelector(`[name=Event${dataKey}]`) as HTMLElement;
            switch (formEl.nodeName) {
                case "INPUT":
                    const inputEle = formEl as HTMLInputElement;
                    inputEle.value = eventReturnData[dataKey];
                    break;

                case "TEXTAREA":
                    const textareaEle = formEl as HTMLTextAreaElement;
                    textareaEle.value = eventReturnData[dataKey];;
                    break;
            }
        }

        const eventDataFromServer: SmUpdateEventDataFromServerDTO = {
            EventDataId: eventReturnData.EventDataId,
            Day: eventReturnData.Day
        }

        const targetEventEl = ev.target as HTMLElement;
        formUpdateEvent.onsubmit = (ev: SubmitEvent) => this.HandleSubmit_UpdateEvent(ev, formUpdateEvent, eventDataFromServer, targetEventEl);

        Utilities.ShowPanel(this._formEditEventLoadingPanelId, this._formEditEventPanelId);
    }
    //#endregion

    //#region Update
    private HandleSubmit_UpdateEvent(ev: SubmitEvent, form: HTMLFormElement, eventDataFromServer: SmUpdateEventDataFromServerDTO, eventEl:HTMLElement): void {
        ev.preventDefault();

        if (!this.CurrentDayCheck(eventDataFromServer.Day)) {
            alert("error selected day does not exist");
        };

        const formData: FormData = new FormData(form);

        const dataToServer: SmEventDataUpdateDTO = {
            EventDataId: eventDataFromServer.EventDataId,
            Day: eventDataFromServer.Day,
            Title: formData.get("EventTitle") as string,
            Info: formData.get("EventInfo") as string,
            Start: formData.get("EventStart") as string,
            End: formData.get("EventEnd") as string,
            Colour: formData.get("EventColour") as string
        };

        const verficationToken = (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value;

        const xhr = new XMLHttpRequest() as XMLHttpRequest;
        xhr.open('POST', this._urlUpdateEvent);
        xhr.setRequestHeader("XSRF-TOKEN", verficationToken);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onload = () => xhr.status === 200 ? this.HandleSubmitDone_UpdateEvent(JSON.parse(xhr.response), form, eventEl) : console.log(xhr, "Failed on HandleSubmit_UpdateEvent");
        Utilities.Sm_XMLHttpRequest(xhr, dataToServer);
    }

    private HandleSubmitDone_UpdateEvent(eventReturnData: SmEventReturnDataDTO, form:HTMLFormElement, eventEl:HTMLElement): void {
        const title = eventEl.querySelector('#Title') as HTMLHeadElement;
        title.textContent = eventReturnData.Title;

        const info = eventEl.querySelector('#Info') as HTMLParagraphElement;
        info.textContent = eventReturnData.Info;

        const backgroundColour = eventEl as HTMLElement;
        backgroundColour.style.backgroundColor = eventReturnData.Colour;

        const startTime = eventEl.querySelector('#StartTime') as HTMLHeadElement;
        startTime.textContent = eventReturnData.Start;

        const endTime = eventEl.querySelector('#EndTime') as HTMLHeadElement;
        endTime.textContent = eventReturnData.End;
        
        this._modalEditEvent.hide();
        form.reset();
    }
    //#endregion
}
/// <reference types="./SmEngine.d.ts" />
/// <reference path="./Utilities.ts" />

class SmEngine {
    private readonly _controller = window.location.pathname + "?handler=";
    private readonly _urlCreateEvent = this._controller + "CreateEvent";
    private readonly _urlUpdateEvent = this._controller + "UpdateEvent";
    private readonly _urlDeleteEvent = this._controller + "DeleteEvent";
    private readonly _urlLoadEvents = this._controller + "LoadEvents";
    private readonly _urlLoadEvent = this._controller + "LoadEvent";

    private readonly _tabDays = document.querySelector('#tabDays') as HTMLUListElement;
    private readonly _tabContentDays = document.querySelector('#tabContentDays') as HTMLDivElement;
    private readonly _dayTabEl = document.querySelector('#dayTabEl') as HTMLLIElement;
    private readonly _dayTabPaneEl = document.querySelector('#dayTabPaneEl') as HTMLDivElement;
    private readonly _dayListGroupEl = document.querySelector('#listGroupEl') as HTMLDivElement;
    private readonly _dayListGroupItemEl = document.querySelector('#listGroupItemEl') as HTMLDivElement;

    private _days: string[] = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    private _currentDay: string = this._days[0];

    private readonly _modalInfo = Utilities.BTSP_GetModal('#modalInfo');
    private readonly _modalInfoMsgEl = document.querySelector('#modalInfo').querySelector('#infoMsg') as HTMLLIElement;

    private readonly _modalCreateEvent = Utilities.BTSP_GetModal('#modalCreateEvent');
    private readonly _formCreateEventId: string = '#formCreateEvent';

    private readonly _modalUpdateEvent = Utilities.BTSP_GetModal('#modalEditEvent');
    private readonly _formDeleteEventId: string = '#formDeleteEvent';
    private readonly _formUpdateEventId: string = '#formUpdateEvent';
    private readonly _formUpdateEventPanelId = 'divEditEventModalFormPanel';
    private readonly _formUpdateEventLoadingPanelId = "divEditEventModalLoadingPanel";
    private readonly _formUpdateEvDeleteWarningCollapse = Utilities.BTSP_GetCollapse('#deleteEventCollapse');

    /* Submit Button Texts */
    private readonly _createEventBtnText = { Default: `<i class="fas fa-save"></i> Save`, Clicked: `<i class="fa-solid fa-spinner fa-spin"></i> Saving...` }
    private readonly _updateEventBtnText = { Default: `<i class="fas fa-save"></i> Update`, Clicked: `<i class="fa-solid fa-spinner fa-spin"></i> Updating...` }
    private readonly _deleteEventBtnText = { Default: `<i class="fa-regular fa-calendar-xmark"></i> Confirm`, Clicked: `<i class="fa-solid fa-spinner fa-spin"></i> Deleting...` }


    //#region Init
    public static Init(): void {
        const smEngine = new SmEngine();
        smEngine.Init();
    }

    private Init(): void {
        this.Init_EventModals();
        this.Display_DayTabs();
        this.GetCurrentTabDay();
        this.LoadFromServer_UserEvents();
        this.BindSubmit_CreateEvent();
    }

    private Init_EventModals(): void {
        /* Modal Create Event*/
        const btnCreateEvent = document.querySelector('#btnCreateEvent') as HTMLButtonElement;
        btnCreateEvent.onclick = () => {
            //clear form
            (document.querySelector(this._formCreateEventId) as HTMLFormElement).reset();
            const btnSubmit = document.querySelector(this._formCreateEventId).querySelector("[type=submit]") as HTMLButtonElement;
            Utilities.EnableBtn(btnSubmit);
            this._modalCreateEvent.show();
        }

        /* Modal Edit Event*/
        const btnCloseEditModal = document.querySelector('#btnCloseEditModal') as HTMLButtonElement;
        btnCloseEditModal.onclick = (ev: MouseEvent) => Utilities.BTSP_CloseCollapse(this._formUpdateEvDeleteWarningCollapse);
    }
    //#endregion

    //#region Helper Functions
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

    private DisplayCurrentDayErrorMessage(modal: bootstrap.Modal): boolean {
        if (!this.CurrentDayCheck(this._currentDay)) {
            Utilities.BTSP_CloseModal(modal);
            this._modalInfoMsgEl.textContent = "An error occurred, please try again.";
            Utilities.BTSP_OpenModal(this._modalInfo);

            return true;
        };

        return false;
    }

    private DisplayFormSubmitErrorMessage(message: string): void{
        this._modalInfoMsgEl.textContent = message;
            Utilities.BTSP_OpenModal(this._modalInfo);
    }

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

    private SortEventsOrder(a, b) {
        const aStart = a.querySelector('#StartTime').textContent;
        const bStart = b.querySelector('#StartTime').textContent;
        const aEnd = a.querySelector('#EndTime').textContent;
        const bEnd = b.querySelector('#EndTime').textContent;
        if (aStart === bStart) {
            return aEnd! > bEnd! ? 1 : -1;
        }
        return aStart! > bStart! ? 1 : -1;
    }

    private ReorderDayEvents(dayGroupEl: HTMLElement): void {
        const eventItems = dayGroupEl.querySelectorAll('[sm-event-item]') as NodeListOf<HTMLLIElement>;

        const sortedEventItems = Array.from(eventItems);
        sortedEventItems.sort((a, b) => this.SortEventsOrder(a, b));
        sortedEventItems.forEach((eventItem) => {
            eventItem.parentNode?.appendChild(eventItem);
        });
    }
    //#endregion

    //#region Save
    private HandleSubmit_CreateEvent(ev: SubmitEvent, form: HTMLFormElement): void {
        ev.preventDefault();

        const btnSubmit = form.querySelector("[type=submit]") as HTMLButtonElement;
        Utilities.DisableBtn(btnSubmit);
        btnSubmit.innerHTML = this._createEventBtnText.Clicked;

        if (this.DisplayCurrentDayErrorMessage(this._modalCreateEvent)) return;

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

    private LoadFromServerDone_CreateEvent(eventReturnData: SmEventFormResponseDTO, form: HTMLFormElement): void {
        const btnSubmit = form.querySelector("[type=submit]") as HTMLButtonElement;
        if (eventReturnData.Message !== null) {
            Utilities.EnableBtn(btnSubmit);
            btnSubmit.innerHTML = this._createEventBtnText.Default;
            this.DisplayFormSubmitErrorMessage(eventReturnData.Message)
            return;
        }
        const smEventData = eventReturnData.SmEventItem as SmEventReturnDataDTO;

        const dayPaneListGroupEl = document.querySelector(`#${smEventData.Day}ListGroupEl`) as HTMLElement;

        //Remove No Events Panel If Exits
        const noEventsPanel = document.querySelector(`#divNoEventsPanel${smEventData.Day}`) as HTMLDivElement;
        if (noEventsPanel !== null) noEventsPanel.remove();

        const _dayPaneListGroupItemEl = this._dayListGroupItemEl.cloneNode(true) as HTMLDivElement;
        _dayPaneListGroupItemEl.removeAttribute('id');
        _dayPaneListGroupItemEl.style.backgroundColor = smEventData.Colour;

        const textColour: string = Utilities.GetTextColourContrast(smEventData.Colour);
        const titleEl = _dayPaneListGroupItemEl.querySelector("#Title") as HTMLHeadElement;
        titleEl.textContent = smEventData.Title;
        titleEl.style.color = textColour;

        const infoEl = _dayPaneListGroupItemEl.querySelector("#Info") as HTMLParagraphElement;
        infoEl.textContent = smEventData.Info;
        infoEl.style.color = textColour;

        const startTime = _dayPaneListGroupItemEl.querySelector('#StartTime') as HTMLHeadElement;
        startTime.textContent = smEventData.Start;
        startTime.style.color = textColour;

        const endTime = _dayPaneListGroupItemEl.querySelector('#EndTime') as HTMLHeadElement;
        endTime.textContent = smEventData.End;
        endTime.style.color = textColour;

        _dayPaneListGroupItemEl.onclick = (ev: MouseEvent) => this.LoadFromServer_UserEvent(ev, smEventData.EventDataId);

        dayPaneListGroupEl.appendChild(_dayPaneListGroupItemEl);

        this.ReorderDayEvents(dayPaneListGroupEl);

        this._modalCreateEvent.hide();

        //clear form
        form.reset();
        btnSubmit.innerHTML = this._createEventBtnText.Default;
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
        Utilities.ShowPanel(this._formUpdateEventPanelId, this._formUpdateEventLoadingPanelId);
        this._modalUpdateEvent.show();

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
        const formUpdateEvent = document.querySelector(this._formUpdateEventId) as HTMLFormElement;
        formUpdateEvent.reset();
        formUpdateEvent.onsubmit = null;

        const formDeleteEvent = document.querySelector(this._formDeleteEventId) as HTMLFormElement;
        formDeleteEvent.onsubmit = null;

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

        const btnUpdateSubmit = formUpdateEvent.querySelector("[type=submit]") as HTMLButtonElement;
        Utilities.EnableBtn(btnUpdateSubmit);
        btnUpdateSubmit.innerHTML = this._updateEventBtnText.Default;

        const btnDeleteSubmit = formDeleteEvent.querySelector("[type=submit]") as HTMLButtonElement;
        Utilities.EnableBtn(btnDeleteSubmit);
        btnDeleteSubmit.innerHTML = this._deleteEventBtnText.Default;

        const targetEventEl = (ev.target as HTMLElement).closest('.list-group-item') as HTMLElement;
        formUpdateEvent.onsubmit = (ev: SubmitEvent) => this.HandleSubmit_UpdateEvent(ev, formUpdateEvent, eventDataFromServer, targetEventEl);
        formDeleteEvent.onsubmit = (ev: SubmitEvent) => this.HandleSubmit_DeleteEvent(ev, formDeleteEvent, eventDataFromServer, targetEventEl);

        Utilities.ShowPanel(this._formUpdateEventLoadingPanelId, this._formUpdateEventPanelId);
    }
    //#endregion

    //#region Update
    private HandleSubmit_UpdateEvent(ev: SubmitEvent, form: HTMLFormElement, eventDataFromServer: SmUpdateEventDataFromServerDTO, eventEl: HTMLElement): void {
        ev.preventDefault();

        const btnSubmit = form.querySelector("[type=submit]") as HTMLButtonElement;
        Utilities.DisableBtn(btnSubmit);
        btnSubmit.innerHTML = this._updateEventBtnText.Clicked;

        if (this.DisplayCurrentDayErrorMessage(this._modalUpdateEvent)) return;

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

    private HandleSubmitDone_UpdateEvent(eventReturnData: SmEventFormResponseDTO, form: HTMLFormElement, eventEl: HTMLElement): void {
        const btnSubmit = form.querySelector("[type=submit]") as HTMLButtonElement;
        if (eventReturnData.Message !== null) {
            Utilities.EnableBtn(btnSubmit);
            btnSubmit.innerHTML = this._createEventBtnText.Default;
            this.DisplayFormSubmitErrorMessage(eventReturnData.Message)
            return;
        }
        
        const smEventData = eventReturnData.SmEventItem as SmEventReturnDataDTO;
        const textColour: string = Utilities.GetTextColourContrast(smEventData.Colour);

        const title = eventEl.querySelector('#Title') as HTMLHeadElement;
        title.textContent = smEventData.Title;
        title.style.color = textColour;

        const info = eventEl.querySelector('#Info') as HTMLParagraphElement;
        info.textContent = smEventData.Info;
        info.style.color = textColour;

        const backgroundColour = eventEl as HTMLElement;
        backgroundColour.style.backgroundColor = smEventData.Colour;
        backgroundColour.style.color = textColour;

        const startTime = eventEl.querySelector('#StartTime') as HTMLHeadElement;
        startTime.textContent = smEventData.Start;
        startTime.style.color = textColour;

        const endTime = eventEl.querySelector('#EndTime') as HTMLHeadElement;
        endTime.textContent = smEventData.End;
        endTime.style.color = textColour;

        this.ReorderDayEvents(eventEl.parentElement);

        this._modalUpdateEvent.hide();
        form.reset();
    }
    //#endregion

    //#region Delete
    private HandleSubmit_DeleteEvent(ev: SubmitEvent, form: HTMLFormElement, eventDataFromServer: SmUpdateEventDataFromServerDTO, eventEl: HTMLElement): void {
        ev.preventDefault();

        const btnSubmit = form.querySelector("[type=submit]") as HTMLButtonElement;
        Utilities.DisableBtn(btnSubmit);
        btnSubmit.innerHTML = this._deleteEventBtnText.Clicked;

        if (this.DisplayCurrentDayErrorMessage(this._modalUpdateEvent)) { Utilities.BTSP_CloseCollapse(this._formUpdateEvDeleteWarningCollapse); return };

        const dataToServer: SmEventDataIdDTO = {
            EventDataId: eventDataFromServer.EventDataId,
        };

        const verficationToken = (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value;

        const xhr = new XMLHttpRequest() as XMLHttpRequest;
        xhr.open('POST', this._urlDeleteEvent);
        xhr.setRequestHeader("XSRF-TOKEN", verficationToken);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onload = () => xhr.status === 200 ? this.HandleSubmitDone_DeleteEvent(JSON.parse(xhr.response), form, eventEl) : console.log(xhr, "Failed on HandleSubmit_UpdateEvent");
        Utilities.Sm_XMLHttpRequest(xhr, dataToServer);
    }

    private HandleSubmitDone_DeleteEvent(statusMessage: SmStatusMessage, form: HTMLFormElement, eventEl: HTMLElement): void {
        this._modalUpdateEvent.hide();
        Utilities.BTSP_CloseCollapse(this._formUpdateEvDeleteWarningCollapse)

        this._modalInfoMsgEl.textContent = "";
        this._modalInfoMsgEl.textContent = statusMessage.Message;
        this._modalInfo.show();

        eventEl.remove();
    }
    //#endregion
}
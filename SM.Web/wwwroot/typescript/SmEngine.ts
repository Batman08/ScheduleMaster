/// <reference types="./SmEngine.d.ts" />
/// <reference path="./Utilities.ts" />

class SmEngine {
    private readonly _controller = window.location.pathname + "?handler=";
    private readonly _urlCreateEvent = this._controller + "CreateEvent";
    private readonly _urlLoadEvents = this._controller + "LoadEvents";

    private readonly _tabDays = document.querySelector('#tabDays') as HTMLUListElement;
    private readonly _tabContentDays = document.querySelector('#tabContentDays') as HTMLDivElement;
    private readonly _dayTabEl = document.querySelector('#dayTabEl') as HTMLLIElement;
    private readonly _dayTabPaneEl = document.querySelector('#dayTabPaneEl') as HTMLDivElement;
    private readonly _dayListGroupEl = document.querySelector('#listGroupEl') as HTMLDivElement;
    private readonly _dayListGroupItemEl = document.querySelector('#listGroupItemEl') as HTMLDivElement;

    private _days: string[] = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    private _currentDay: string = 'Monday';

    private Init(): void {
        //if (Utilities.GetCookie("")) {
        //    Utilities.SetCookie("", '', -1);
        //}
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
        xhr.onload = () => xhr.status === 200 ? console.log(JSON.parse(xhr.response)) : console.log(xhr, "Failed on HandleSubmit_CreateEvent");
        Utilities.Sm_XMLHttpRequest(xhr, dataToServer);
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

        //this.LoadFromServerDone_UserEvents();
    }

    private LoadFromServerDone_UserEvents(eventReturnData: SmEventReturnDataDTO[]): void {
        eventReturnData.forEach(eventData => {
            const _dayPaneListGroupItemEl = this._dayListGroupItemEl.cloneNode(true) as HTMLDivElement;
            _dayPaneListGroupItemEl.id = ``;
            _dayPaneListGroupItemEl.querySelector("#Title").textContent = eventData.Title;
            _dayPaneListGroupItemEl.querySelector("#Info").textContent = eventData.Info;

            const dayPaneListGroupEl = document.querySelector(`#${eventData.Day}ListGroupEl`);
            dayPaneListGroupEl.appendChild(_dayPaneListGroupItemEl);            
        });

        //this._days.forEach(day => {
        //    const _dayPaneListGroupItemEl = this._dayListGroupItemEl.cloneNode(true) as HTMLDivElement;
        //    _dayPaneListGroupItemEl.id = ``;
        //    _dayPaneListGroupItemEl.querySelector("#Title").textContent = day;
        //    _dayPaneListGroupItemEl.querySelector("#Info").textContent = "";

        //    const dayPaneListGroupEl = document.querySelector(`#${day}ListGroupEl`);
        //    dayPaneListGroupEl.appendChild(_dayPaneListGroupItemEl);
        //});
    }
    //#endregion
}
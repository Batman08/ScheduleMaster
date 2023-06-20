﻿/// <reference types="./SmEngine.d.ts" />
/// <reference path="./Utilities.ts" />

class SmEngine {
    private readonly _controller = window.location.pathname + "?handler=";
    private readonly _urlCreateEvent= this._controller + "CreateEvent";
    
    private readonly _tabDays = document.querySelector('#tabDays') as HTMLUListElement;
    private readonly _tabContentDays = document.querySelector('#tabContentDays') as HTMLDivElement;
    private readonly _dayTabEl = document.querySelector('#dayTabEl') as HTMLLIElement;
    private readonly _dayTabPaneEl = document.querySelector('#dayTabPaneEl') as HTMLDivElement;

    private _days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']

    private Init(): void {
        this.Display_DayTabs();
        this.BindSubmit_CreateEvent();
    }

    public static Init(): void {
        const smEngine = new SmEngine();
        smEngine.Init();
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

        console.log(dayTabEl);

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

        this._tabDays.appendChild(dayTabEl);
        this._tabContentDays.appendChild(daytabPaneEl);
    }

    private HandleSubmit_CreateEvent(ev: SubmitEvent, form: HTMLFormElement): void {
        ev.preventDefault();
        
        const formData: FormData = new FormData(form);

        const dataToServer:SmEventDataDTO = {
            Title: formData.get("EventTitle") as string,
            Info: formData.get("EventInfo") as string
        };
        
        const verficationToken = (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value;

        const xhr = new XMLHttpRequest() as XMLHttpRequest;
        xhr.open('POST', this._urlCreateEvent);
        xhr.setRequestHeader("XSRF-TOKEN", verficationToken);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onload = () => xhr.status === 200 ? console.log(JSON.parse(xhr.response)) : console.log(xhr, "Failed on HandleSubmit_CreateEvent");
        Utilities.Sm_XMLHttpRequest(xhr, dataToServer);
    }

    private BindSubmit_CreateEvent(): void
    {
        const form = document.querySelector('#formCreateEvent') as HTMLFormElement;
        form.onsubmit = (ev: SubmitEvent) => this.HandleSubmit_CreateEvent(ev, form);
    }
}
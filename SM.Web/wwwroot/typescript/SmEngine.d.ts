type SmEventDataDTO = {
    Day: string;
    Title: string;
    Info: string;
    Start: string;
    End: string;
    Colour: string;
}

type SmUpdateEventDataFromServerDTO = {
    EventDataId: string;
    Day: string;
}

type SmEventDataUpdateDTO = {
    EventDataId: string;
    Day: string;
    Title: string;
    Info: string;
    Start: string;
    End: string;
    Colour: string;
}

type SmEventReturnDataDTO = {
    EventDataId: string;
    Day: string;
    Title: string;
    Info: string;
    Start: string;
    End: string;
    Colour: string;
}

type SmEventFormResponseDTO = {
    SmEventItem: SmEventReturnDataDTO;
    Message: string | null;
}

type SmEventDataIdDTO = {
    EventDataId: string;
}

type SmStatusMessage = {
    Message: string;
}
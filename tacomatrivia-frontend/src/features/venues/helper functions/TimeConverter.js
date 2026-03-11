export default function TimeConverter (venueTime){
    
    // split it into hours minutes and seconds
    const[hours,minutes, seconds] = venueTime.split(":");

    //set a new date object and set it as such
    const dummyDate = new Date();
    dummyDate.setHours(hours, minutes, seconds);
    //Format to 12 hour with pm/ am

    const time = dummyDate.toLocaleTimeString('en-US', {
        hour: 'numeric',
        minute: 'numeric',
        hour12: true
    });

    return time;
}
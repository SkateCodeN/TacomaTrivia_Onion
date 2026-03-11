
// False means that the event has passed 
export default function CompareDateTime(venueTime)
{
    //Get the current date, we are comparing times
    const currentDateTime = new Date();

    const venueDateTime = new Date();
    const [hours, minutes, seconds] = venueTime.split(":");
    venueDateTime.setHours(hours, minutes, seconds);
    //if the currentDateTime is more than the venueDT then the trivia event is passed
    if( currentDateTime > venueDateTime)
    {
        return false;
    }
    else{
        return true;
    }
}
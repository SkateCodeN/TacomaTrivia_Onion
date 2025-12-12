import {useAuth0} from "@auth0/auth0-react"
export default function HomePage(){
    const {loginWithRedirect, logout, getAccessTokenSilently, isAuthenticated, user} = useAuth0();

  const callPrivateApi = async() =>{
    const token = await getAccessTokenSilently();
    console.log("TOKEN:", token);
    const res = await fetch ("/api/venues/privateTest",{
      headers:{Authorization: `Bearer ${token}`}
    });
    const text = await res.text();
    console.log("Got to call the private api")
    alert(text);
  }
  
    return(
        <>
            <h1>Hello Auth User!</h1>
            <p> Lets make an auth test to our backend</p>
            <button onClick={() => callPrivateApi()}>Test Backend</button>
        </>
    )
}
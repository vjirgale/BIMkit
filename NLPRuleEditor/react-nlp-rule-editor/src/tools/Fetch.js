

//https://javascript.info/fetch
//fetchs data from url.
//returns data in JSON format.
export async function fetchData(url){
    console.log("...");
    let response = await fetch(url,{
      method: 'GET'
    });
    if(response.ok){
      //parse data
      let result = await response.json();
      //return data
      return result;
    }
    else{
      alert("HTTP-Error: " + response.status);
    }
  }
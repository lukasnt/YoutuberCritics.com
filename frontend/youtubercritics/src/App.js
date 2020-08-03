import React, { useState } from 'react';
import './App.css';
import Navbar from "./components/navbar";
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import TrendingChannels from './components/trendingchannels/TrendingChannels';
import Grid from "@material-ui/core/Grid";
import ChannelPage from './components/channel/ChannelPage';
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import axios from 'axios';
import SearchPage from './components/search/searchpage';
import RecentReviews from './components/reviews/RecentReviews';

function App() {
  const theme = createMuiTheme({
    palette: {
      primary: {
        // light: will be calculated from palette.primary.main,
        main: '#ffffff',
        // dark: will be calculated from palette.primary.main,
        // contrastText: will be calculated to contrast with palette.primary.main
      },
      secondary: {
        main: '#ca2014',
        //main: '#000032',
        //main: '#320000',
        // dark: will be calculated from palette.secondary.main,
      },
      // Used by `getContrastText()` to maximize the contrast between
      // the background and the text.
      contrastThreshold: 3,
      // Used by the functions below to shift a color's luminance by approximately
      // two indexes within its tonal palette.
      // E.g., shift from Red 500 to Red 300 or Red 700.
      tonalOffset: 0.2,
    },
  });

  return (
    <div>
      <ThemeProvider theme={theme}>
        <Router>
        <Navbar />
            <Switch>
              <Route path="/channel" component={(props) => <ChannelPage youtubePath={props.location.search.slice(1)} />} /> 
              <Route path="/search" component={(props) => <SearchPage params={props.location.search.slice(1)} />} />
              <Route path="/"> <Home /> </Route>
            </Switch>
        </Router>
      </ThemeProvider>
    </div>
    );
}

const backendDomain = "https://localhost:5001"; // "https://youtubercritics.azurewebsites.net"; // "https://localhost:5001";
const frontendDomain = "http://localhost:3000"; // "https://youtubercritics.com"; // "http://localhost:3000";
function redirect(path) {
  window.history.pushState(null, "", window.location.href);
  window.location.replace(frontendDomain + path);
}
document.title = "Home - YoutuberCritics";

function Home() {
  const[channels, setChannels] = useState([]);
  const[sent, setSent] = useState(false);

  if (!sent){
    axios.get(backendDomain + "/api/channels?maxItems=8")
      .then(res => setChannels(res.data));
    setSent(true);
  }
  return (
    <Container>
      <Grid container spacing={3}>
        <Grid item> <RecentReviews /> </Grid>
        <Grid item> <TrendingChannels channels={channels}/> </Grid>
      </Grid>
    </Container>
  )
}

export default App;
export { backendDomain, frontendDomain, redirect };

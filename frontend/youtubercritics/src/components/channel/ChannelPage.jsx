import React, { useState } from "react";
import axios from "axios";
import qs from "qs";
import { AppBar, Container, Grid, Paper, Typography, Button, TextField } from "@material-ui/core";
import Rating from "@material-ui/lab/Rating"
import PersonIcon from '@material-ui/icons/Person';

import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Reviews from "../reviews/Reviews";
import { backendDomain } from "../../App";

export default function ChannelPage( {youtubePath} ) {
    const [channel, setChannel] = useState(
        {
            channelID: -1,
            description: "",
            imageURL: "",
            title: "",
            youtubePath: "",
        }
    );
    const [reviews, setReviews] = useState([]);
    const [sent, setSent] = useState(false);
    
    console.log(youtubePath)
    let path = qs.parse(youtubePath).pathName;

    if (!sent) {
        axios.get(backendDomain + "/api/channels/user?pathName=" + path)
          .then(res => setChannel(res.data));
        
        axios.get(backendDomain + "/api/reviews?pathName=" + path)
        .then(res => setReviews(res.data));
        
        setSent(true);
    }

    console.log(channel);

    return (
        <div>
            <AppBar position="relative" style={{zIndex: 0, top: -10, height: 300, alignItems: "center", verticalAlign: "center"}}>
                <Grid container
                    direction="row"
                    justify="center"
                    alignItems="center"
                    spacing={2}
                    style={{maxWidth:"1000px", position: "relative", flexWrap: "nowrap", top: "20%"}}
                    >
                    <Grid item> 
                        <a href={"https://youtube.com/" + channel.youtubePath}> 
                            <img src={channel.imageURL} alt={channel.title}/> 
                        </a>
                    </Grid>
                    <Grid item>
                        <Typography variant="h4" > {channel.title} </Typography>
                        <Typography variant="subtitle1"> 500 Reviews </Typography>
                        <Rating 
                            precision={0.1} 
                            value={4.6} 
                            name="Rating" 
                            style={{fontSize: "3rem"}} size="large"
                            readOnly/>
                        <Typography> {channel.description} </Typography>
                    </Grid>
                </Grid>
            </AppBar>

            <Container>
                <Grid container spacing={3}>
                    <Grid item xs={9}>
                        <Grid container spacing={2}>
                        <Grid item style={{width: "100%"}}> 
                            <Paper>
                                <PostReview channel={channel}/>
                            </Paper> 
                        </Grid>
                        <Grid item xs={12}>
                            <Reviews reviews={reviews} style={{width: "100%"}} />
                        </Grid>
                            
                        </Grid> 
                    </Grid> 
                    <Grid item xs={3}> 
                        <Paper style={{height: "500px"}}>
                            <Typography> 
                                Ads
                            </Typography>
                        </Paper>
                    </Grid>
                </Grid>
            </Container>
        </div>
    );
}

function PostReview( {channel} ) {
    const [rating, setRating] = useState(0);

    function handlePost(event) {
        let reviewTitle = document.getElementById("reviewTitle").value;
        let reviewText = document.getElementById("reviewText").value;

        axios.post(backendDomain + "/api/channels/" + channel.channelID + "/reviews", 
        {
            channelID: channel.channelID,
            userID: 1,
            rating: rating,
            title: reviewTitle,
            text: reviewText,
        }
        ).then(res => {
            setRating(0);
            reviewText = "";
            reviewTitle = "";
            console.log("POSTED", channel.channelID, rating, reviewText);
            window.location.replace("http://localhost:3000/channel/user?pathName=" + channel.youtubePath);
        });
    }

    return (
        <Accordion>
            <AccordionSummary 
                style={{
                    padding: "16px", 
                    display: "flex", 
                    direction: "row", 
                    alignItems: "center", 
                    justifyContent: "space-between"
                }}>
                <PersonIcon fontSize="large"/>
                <Button variant="contained" color="secondary" style={{width: "100%"}}> Write a review </Button>
            </AccordionSummary>
            <AccordionDetails style={{padding: "16px", display: "flex", direction: "column", flexWrap: "wrap"}}>
                <Rating
                    name="PostRating"
                    value={rating}
                    onChange={(event, newValue) => {
                        setRating(newValue);
                    }}
                    style={{fontSize: "3rem"}} 
                    size="large" 
                />
                <TextField
                    id="reviewTitle"
                    fullWidth
                    label="Title"
                    variant="filled"
                />
                <TextField
                    id="reviewText"
                    fullWidth
                    multiline
                    label="Write your review"
                    variant="filled"
                    rows={10}
                />
                <Button variant="contained" color="secondary" onClick={handlePost}> Post </Button>
            </AccordionDetails>
        </Accordion>
    );
}
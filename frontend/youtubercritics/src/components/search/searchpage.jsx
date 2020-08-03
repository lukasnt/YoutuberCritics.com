import React, { useState } from "react";
import { Paper, Tabs, Tab, MenuItem, Select, FormControl, Grid, Container, Typography, CircularProgress, useMediaQuery } from "@material-ui/core";
import ChannelCard from "../channel/ChannelCard";
import Pagination from '@material-ui/lab/Pagination';
import axios from "axios";
import qs from 'qs';
import { backendDomain, redirect } from "../../App";
import Reviews from "../reviews/Reviews";

export default function SearchPage( { params } ) {
    const mobile = useMediaQuery("(max-width: 600px)");
    
    let parsedParams = qs.parse(params);
    let term = parsedParams.keyword;
    let initTab = parseInt(parsedParams.tab);
    let initChannelOrder = parseInt(parsedParams.channelOrder);
    let initReviewOrder = parseInt(parsedParams.reviewOrder);
    let initChannelPage = parseInt(parsedParams.channelPage);
    let initReviewPage = parseInt(parsedParams.reviewPage);
    const initLoad = (
        <Grid item xs={12} size="large" style={{display: "flex", justifyContent: "center"}}>
            <CircularProgress style={{
                width: "150px",
                height: "150px",
            }} color="secondary"/> 
        </Grid>);

    const pageSize = 15;
    const [sent, setSent] = useState(false);
    const [channels, setChannels] = useState([]);
    const [reviews, setReviews] = useState([]);
    const [loading, setLoading] = useState(initLoad);
    const [tab] = useState(!isNaN(initTab) ? initTab : 0);
    const [reviewOrder, setReviewOrder] = useState(!isNaN(initReviewOrder) ? initReviewOrder : 0);
    const [channelOrder, setChannelOrder] = useState(!isNaN(initChannelOrder) ? initChannelOrder : 0);
    const [channelPage, setChannelPage] = useState(!isNaN(initChannelPage) ? initChannelPage : 1);
    const [reviewPage, setReviewPage] = useState(!isNaN(initReviewPage) ? initReviewPage : 1);


    if (!sent) {
        if (tab === 0) {
            axios.get(backendDomain + "/api/search?keyword=" + term + "&order=" + channelOrder + "&page=" + channelPage + "&pageSize=" + pageSize + "&scrape=" + (channelPage === 1 && channelOrder === 0 && term !== "" ? "true" : "false"))
                .then(res => {
                    setChannels(res.data.splice(0, pageSize));
                    setLoading(null);
                });
        } else if (tab === 1) {
            axios.get(backendDomain + "/api/search/reviews?keyword=" + term + "&order=" + reviewOrder + "&page=" + reviewPage + "&pageSize=" + pageSize)
                .then(res => {
                    setReviews(res.data.splice(0, pageSize));
                    setLoading(null);
                });
            }
        setSent(true);    
    }

    let channelItems = channels.map(channel => (
            <Grid item>
                <ChannelCard channel={channel} />
            </Grid>
        )    
    );
    let reviewItems = <Reviews reviews={reviews} />;
    
    function internalRedirect(term, tab, reviewOrder, reviewPage, channelOrder, channelPage) {  
        redirect("/search?keyword=" + term + "&tab=" + tab + "&reviewOrder=" + reviewOrder + "&reviewPage=" + reviewPage + "&channelOrder=" + channelOrder + "&channelPage=" + channelPage);
    }
    
    const handleTabChange = (event, newValue) => {
        //setTab(newValue);
        internalRedirect(term, newValue, reviewOrder, reviewPage, channelOrder, channelPage);
    };
    
    const handleReviewOrderChange = (event) => {
        setReviewOrder(event.target.value);
        setLoading(initLoad);
        setReviews([]);
        
        internalRedirect(term, tab, event.target.value, reviewPage, channelOrder, channelPage);
    };
    const handleChannelOrderChange = (event) => {
        setChannelOrder(event.target.value);
        setLoading(initLoad);
        setChannels([]);
        
        internalRedirect(term, tab, reviewOrder, reviewPage, event.target.value, channelPage);
    };

    const handleChannelPageChange = (event, value) => {
        setChannelPage(value);
        setLoading(initLoad);
        setChannels([]);

        internalRedirect(term, tab, reviewOrder, reviewPage, channelOrder, value);
    };
    const handleReviewPageChange = (event, value) => {
        setReviewPage(value);
        //setLoading(initLoad);
        //setReviews([]);

        internalRedirect(term, tab, reviewOrder, value, channelOrder, channelPage);
    };

    document.title = (term ? term : (tab === 0 ? "Channels" : "Reviews")) + " - YoutuberCritics";

    return (
        <Container style={{
            marginTop: "10px"
        }}>
            <Grid container spacing={3} wrap={mobile ? "wrap-reverse" : "wrap"}>
                <Grid item xs={mobile ? 12 : 9}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <Paper style={{
                                display: "flex",
                                direction: "row",
                                alignItems: "center",
                                justifyContent: "space-around"
                            }}>
                                <Tabs
                                    indicatorColor="secondary"
                                    textColor="secondary"
                                    value={tab}
                                    onChange={handleTabChange}
                                >
                                    <Tab label="Channels" />
                                    <Tab label="Reviews" />
                                </Tabs>
                                {
                                    tab === 0 ?
                                        <FormControl >
                                            <Select
                                                value={channelOrder}
                                                onChange={handleChannelOrderChange}
                                            >
                                            <MenuItem value={0}> Subscribers </MenuItem>
                                            <MenuItem value={1}> Name </MenuItem>
                                            <MenuItem value={2}> Rating </MenuItem>
                                            <MenuItem value={3}> Review Count </MenuItem>
                                            <MenuItem value={4}> YTC ID </MenuItem>
                                            </Select>
                                        </FormControl>
                                    :
                                        <FormControl >
                                            <Select
                                                value={reviewOrder}
                                                onChange={handleReviewOrderChange}
                                            >
                                            <MenuItem value={0}> Date Posted </MenuItem>
                                            <MenuItem value={1}> Title </MenuItem>
                                            <MenuItem value={2}> Rating </MenuItem>
                                            <MenuItem value={3}> User </MenuItem>
                                            <MenuItem value={4}> Channel </MenuItem>
                                            </Select>
                                        </FormControl>
                                }
                            </Paper>
                        </Grid>
                            {loading}
                            {tab === 0 ? channelItems : reviewItems}
                        </Grid>
                </Grid>
                <Grid item xs={mobile ? 12 : 3}> 
                        <Paper style={mobile ? {height: "100px"} : {height: "500px"}}>
                            <Typography> 
                                Ads
                            </Typography>
                        </Paper>
                    </Grid>
                <Grid item xs={mobile ? 12 : 9} style={{display: "flex", justifyContent: "center"}}>
                    {
                        tab === 0 ?
                            <Pagination color="secondary" page={channelPage} count={10} onChange={handleChannelPageChange}/>
                        :
                            <Pagination color="secondary" page={reviewPage} count={10} onChange={handleReviewPageChange}/>
                    }
                </Grid>
            </Grid>
        </Container>
    )
}
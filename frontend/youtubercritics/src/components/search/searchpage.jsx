import React, { useState } from "react";
import { Paper, Tabs, Tab, MenuItem, Select, FormControl, Grid, Container, Typography, CircularProgress, useMediaQuery } from "@material-ui/core";
import ChannelCard from "../channel/ChannelCard";
import Pagination from '@material-ui/lab/Pagination';
import axios from "axios";
import qs from 'qs';
import { backendDomain } from "../../App";
import Reviews from "../reviews/Reviews";

export default function SearchPage( { params } ) {
    const mobile = useMediaQuery("(max-width: 600px)")
    
    const pageSize = 15;
    const [sent, setSent] = useState(false);
    const [channels, setChannels] = useState([]);
    const [reviews, setReviews] = useState([]);

    const initLoad = (
    <Grid item xs={12} size="large" style={{display: "flex", justifyContent: "center"}}>
        <CircularProgress style={{
            width: "150px",
            height: "150px",
        }} color="secondary"/> 
    </Grid>);

    const [loading, setLoading] = useState(initLoad);

    let term = qs.parse(params).keyword;
    let initTab = parseInt(qs.parse(params).tab);

    console.log(initTab);
    console.log(isNaN(initTab))

    if (!sent) {
        axios.get(backendDomain + "/api/search?keyword=" + term)
            .then(res => {
                setChannels(res.data.splice(0, pageSize));
                setLoading(null);
            });
        /*
        axios.get(backendDomain + "/api/search?keyword=" + term + "&fullScan=false&scrape=true")
            .then(res => {
                let result = res.data.concat(dbResult);
                result = result.slice(0, 27);
                setChannels(result);
                setLoading(null);
            });
        */
        axios.get(backendDomain + "/api/search/reviews?keyword=" + term)
            .then(res => setReviews(res.data.splice(0, pageSize)))
        setSent(true);
    }

    let channelItems = channels.map(channel => (
            <Grid item>
                <ChannelCard channel={channel} />
            </Grid>
        )    
    );
    let reviewItems = <Reviews reviews={reviews} />;
    
    
    const [tab, setTab] = useState(!isNaN(initTab) ? initTab : 0);
    const handleTabChange = (event, newValue) => {
        setTab(newValue);
    };
    
    const [reviewOrder, setReviewOrder] = useState(0);
    const [channelOrder, setChannelOrder] = useState(0);
    const handleReviewOrderChange = (event) => {
        setReviewOrder(event.target.value);
        setLoading(initLoad);
        setReviews([]);

        axios.get(backendDomain + "/api/search/reviews?keyword=" + term + "&order=" + event.target.value + "&page=" + reviewPage + "&pageSize=" + pageSize)
            .then(res => {
                setReviews(res.data.splice(0, pageSize));
                setLoading(null);
            });
    };
    const handleChannelOrderChange = (event) => {
        setChannelOrder(event.target.value);
        setLoading(initLoad);
        setChannels([]);
        
        axios.get(backendDomain + "/api/search?keyword=" + term + "&order=" + event.target.value + "&page=" + channelPage + "&pageSize=" + pageSize)
            .then(res => {
                setChannels(res.data.splice(0, pageSize));
                setLoading(null);
            });
    };

    const [channelPage, setChannelPage] = useState(1);
    const handleChannelPageChange = (event, value) => {
        setChannelPage(value);
        setLoading(initLoad);
        setChannels([]);

        axios.get(backendDomain + "/api/search?keyword=" + term + "&order=" + channelOrder + "&page=" + value + "&pageSize=" + pageSize)
            .then(res => {
                setChannels(res.data.splice(0, pageSize));
                setLoading(null);
            });
    };
    const [reviewPage, setReviewPage] = useState(1);
    const handleReviewPageChange = (event, value) => {
        setReviewPage(value);
        setLoading(initLoad);
        setReviews([]);

        axios.get(backendDomain + "/api/search/reviews?keyword=" + term + "&order=" + reviewOrder + "&page=" + value + "&pageSize=" + pageSize)
            .then(res => {
                setReviews(res.data.splice(0, pageSize));
                setLoading(null);
            });
    };

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
                                            <MenuItem value={3}> Youtube Path </MenuItem>
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
                                            <MenuItem value={5}> ReviewID </MenuItem>
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
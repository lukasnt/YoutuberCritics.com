import React, { useState } from "react";
import { Typography, Grid, CircularProgress } from "@material-ui/core";
import Paper from '@material-ui/core/Paper';
import ChannelCard from "../channel/ChannelCard";

export default function TrendingChannels( {channels} ) {
    const [loading, setLoading] = useState(
        <Grid item xs={12} size="large" style={{display: "flex", justifyContent: "center"}}>
        <CircularProgress style={{
            width: "150px",
            height: "150px",
        }} color="secondary"/> 
        </Grid>
    );
    if (channels != null & channels.length !== 0 & loading != null) setLoading(null);
   
    var channelList = channels.map(c => 
            <Grid item> <ChannelCard channel={c}/> </Grid>
        );
    
    return (
        <Paper elevation={2} style={{padding: 20}}>
            {loading}
            <Typography variant="h6"> Trending Channels </Typography>
            <Grid 
                container 
                direction="row"
                justify="flex-start"
                alignItems="center" 
                spacing={2}
                > 
                {channelList}
            </Grid>
        </Paper>
    );
}
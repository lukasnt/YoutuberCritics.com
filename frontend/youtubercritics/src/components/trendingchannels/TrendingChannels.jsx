import React from "react";
import { Typography, Grid } from "@material-ui/core";
import Paper from '@material-ui/core/Paper';
import ChannelCard from "../channel/ChannelCard";

export default function TrendingChannels( {channels} ) {
    var channelList = channels.map(c => 
            <Grid item> <ChannelCard channel={c}/> </Grid>
        );
    
    return (
        <Paper elevation={2} style={{padding: 20}}>
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
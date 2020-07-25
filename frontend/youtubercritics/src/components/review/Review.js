import React from "react";
import Card from "@material-ui/core/Card";
import { CardContent, CardHeader, Typography, CardActions, IconButton } from "@material-ui/core";
import Rating from "@material-ui/lab/Rating";
import PersonIcon from '@material-ui/icons/Person';
import FavoriteIcon from '@material-ui/icons/Favorite';
import ShareIcon from '@material-ui/icons/Share';

export default function Review( {review} ) {
    return (
        <Card style={{
            width: "585px",
        }}>
            <CardHeader
                avatar = {
                    <div>
                        <Rating name="rating" style={{fontSize: "31px"}} value={review.rating} size="large" readOnly/>
                    </div>
                }
                title={
                    <div style={{display: "flex", direction: "row"}}>
                        <PersonIcon style={{position: "relative", top: "5px"}}/>
                        <Typography> <b> {review.user.name}</b> Reviewed <b><img width="24px" height="24px" src={review.channel.imageURL} alt={review.channel.title} style={{position: "relative", top: "5px"}}/>{review.channel.title}</b> </Typography>
                    </div>
                }
                action={
                    <Typography variant="subtitle2"> {review.datePosted} </Typography>
                }
            />
            <CardContent style={{paddingTop: "0px", paddingBottom: "0px"}}>
                <Typography variant="h5"> {review.title != null ? review.title : ""} </Typography>
                <Typography> {review.text} </Typography>
            </CardContent>
            <CardActions>
                <IconButton aria-label="add to favorites">
                    <FavoriteIcon style={{fontSize: "18px"}}/>
                </IconButton>
                <IconButton aria-label="share">
                    <ShareIcon style={{fontSize: "18px"}}/>
                </IconButton>
            </CardActions>
        </Card>
    );
}
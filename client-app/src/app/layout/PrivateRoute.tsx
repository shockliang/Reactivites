import {useStore} from "../stores/store";
import {Redirect, Route, RouteComponentProps, RouteProps} from "react-router-dom";
import React from "react";

interface Props extends RouteProps {
    component: React.ComponentType<RouteComponentProps<any>> | React.ComponentType<any>
}

export default function PrivateRoute({component: Component, ...rest}: Props) {
    const {userStore: {isLoggedIn}} = useStore();

    return (
        <Route
            {...rest}
            render={(props) => isLoggedIn ? <Component {...props} /> : <Redirect to={'/'}/>}
        />
    )
}

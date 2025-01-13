import React from "react";
import { useMsal } from "@azure/msal-react";
import LogoutButton from "../LogoutButton/LogoutButton";

function Home() {
  const { instance } = useMsal();

  const onLogout = () => {
    instance.logoutRedirect();
  };

  return (
    <div>
      Welcome!
      <LogoutButton onLogout={onLogout}></LogoutButton>
    </div>
  );
}

export default Home;

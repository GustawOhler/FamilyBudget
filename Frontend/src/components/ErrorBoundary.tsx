import React from "react";

type Props = {
  children: JSX.Element;
};

type State = {
  hasError: boolean;
};

class ErrorBoundary extends React.Component<Props, State> {
  state: State = { hasError: false };

  static getDerivedStateFromError(error: any) {
    // Update state so the next render will show the fallback UI.
    return { hasError: true };
  }

  componentDidCatch(error: Error, info: any) {
    console.log(error);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div>Something is wrong!</div>
        // <ErrorPopup
        //   isShow={this.state.hasError}
        //   onCloseModal={() => {
        //     this.setState({ hasError: false });
        //   }}
        // />
      );
    }

    return <>{this.props.children}</>;
  }
}

export default ErrorBoundary;
